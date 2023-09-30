using System.Net.Http;
using Conduit.Features.Profiles;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Security;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MediatR;
using System.Reflection;
using System;

namespace Conduit.IntegrationTests
{
    public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient client;
        private readonly string _defaultDatabaseConnectionString = Guid.NewGuid() + ".db";//"Filename=realworldtest.db";
        public IntegrationTestBase(WebApplicationFactory<Program> factory)
            => client = factory
                .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                {
                    services.AddEndpointsApiExplorer();
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
                    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
                    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DBContextTransactionPipelineBehavior<,>));
                    services.AddDbContext<ConduitContext>(options => options.UseSqlite(_defaultDatabaseConnectionString));
                    //services.AddCors();
                    services.Configure<JsonOptions>(opt => opt.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);
                    services.AddFluentValidationAutoValidation();
                    services.AddFluentValidationClientsideAdapters();
                    services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
                    services.AddAutoMapper(typeof(Program));
                    services.AddScoped<IPasswordHasher, PasswordHasher>();
                    services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
                    services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
                    services.AddScoped<IProfileReader, ProfileReader>();
                    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    //services.AddJwt();
                }

                ))
                .CreateClient();
    }
}
