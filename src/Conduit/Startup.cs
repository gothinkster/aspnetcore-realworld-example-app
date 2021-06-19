using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Conduit.Features.Profiles;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using Conduit.Infrastructure.Security;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Conduit
{
    public class Startup
    {
        public const string DEFAULT_DATABASE_CONNECTIONSTRING = "Filename=realworld.db";
        public const string DEFAULT_DATABASE_PROVIDER = "sqlite";

        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DBContextTransactionPipelineBehavior<,>));

            // take the connection string from the environment variable or use hard-coded database name
            var connectionString = _config.GetValue<string>("ASPNETCORE_Conduit_ConnectionString") ??
                                   DEFAULT_DATABASE_CONNECTIONSTRING;
            // take the database provider from the environment variable or use hard-coded database provider
            var databaseProvider = _config.GetValue<string>("ASPNETCORE_Conduit_DatabaseProvider");
            if (string.IsNullOrWhiteSpace(databaseProvider))
            {
                databaseProvider = DEFAULT_DATABASE_PROVIDER;
            }

            services.AddDbContext<ConduitContext>(options =>
            {
                if (databaseProvider.ToLowerInvariant().Trim().Equals("sqlite", StringComparison.Ordinal))
                {
                    options.UseSqlite(connectionString);
                }
                else if (databaseProvider.ToLowerInvariant().Trim().Equals("sqlserver", StringComparison.Ordinal))
                {
                    // only works in windows container
                    options.UseSqlServer(connectionString);
                }
                else
                {
                    throw new InvalidOperationException("Database provider unknown. Please check configuration");
                }
            });

            services.AddLocalization(x => x.ResourcesPath = "Resources");

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                x.SupportNonNullableReferenceTypes();

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {   new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()}
                });
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "RealWorld API", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => new List<string>()
                {
                    y.GroupName ?? throw new InvalidOperationException()
                });
            });

            services.AddCors();
            services.AddMvc(opt =>
                {
                    opt.Conventions.Add(new GroupByApiRootConvention());
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                    opt.EnableEndpointRouting = false;
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddFluentValidation(cfg =>
                {
                    cfg.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddAutoMapper(GetType().Assembly);

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped<IProfileReader, ProfileReader>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilogLogging();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();
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

            app.ApplicationServices.GetRequiredService<ConduitContext>().Database.EnsureCreated();
        }
    }
}
