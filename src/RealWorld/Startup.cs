using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RealWorld.Infrastructure;
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
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddAutoMapper();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("somethinglongerforthisdumbalgorithmisrequired"));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = "issuer";
                options.Audience = "Audience";
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            Mapper.AssertConfigurationIsValid();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseCors(builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            var options = app.ApplicationServices.GetRequiredService<IOptions<JwtIssuerOptions>>();
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.Value.SigningCredentials.Key,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = options.Value.Issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = options.Value.Audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters,
                AuthenticationScheme = JwtIssuerOptions.Scheme,
                Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        //Have to modify request since the standard for this project uses Token instead of Bearer
                        string auth = context.Request.Headers["Authorization"];
                        if (auth?.StartsWith("Token ", StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            context.Request.Headers["Authorization"] = "Bearer " + auth.Substring("Token ".Length).Trim();
                        }
                        return Task.CompletedTask;
                    }
                }
            });
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
