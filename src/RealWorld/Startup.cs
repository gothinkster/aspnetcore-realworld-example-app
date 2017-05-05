using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;
using RealWorld.Infrastructure.Security;
using Swashbuckle.AspNetCore.Swagger;

namespace RealWorld
{
    public class Startup
    {
        public const string DATABASE_FILE = "realworld.db";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR();

            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<RealWorldContext>();
            
            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "RealWorld API", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => y.GroupName);
            });

            services.AddCors();
            services.AddMvc(opt =>
                {
                    opt.Conventions.Add(new GroupByApiRootConvention());
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddAutoMapper();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddJwt();

            Mapper.AssertConfigurationIsValid();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseJwt();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "RealWorld API V1");
            });
            
            app.ApplicationServices.GetRequiredService<RealWorldContext>().Database.EnsureCreated();
        }
    }
}
