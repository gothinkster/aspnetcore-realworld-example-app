using System;
using System.IO;
using System.Threading.Tasks;
using Conduit.Features.Profiles;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Security;
using FluentValidation.AspNetCore;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Http.Json;
using System.Reflection;

namespace Conduit.IntegrationTests
{
    [CollectionDefinition(nameof(SliceFixture))]
    public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }
    public class SliceFixture : IAsyncLifetime
    {
        //private static readonly IConfiguration CONFIG;


        private static IServiceScopeFactory _scopeFactory;
        private static ServiceProvider _provider;
        private readonly string _dbName = "Filename=realworldtest.db";
        //static SliceFixture() => CONFIG = new ConfigurationBuilder()
        //       .AddEnvironmentVariables()
        //       .Build();

        public async Task InitializeAsync()
        {
            var builder = WebApplication.CreateBuilder();

            var services = builder.Services;
            services.AddEndpointsApiExplorer();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DBContextTransactionPipelineBehavior<,>));
            services.AddDbContext<ConduitContext>(options => options.UseInMemoryDatabase(_dbName));
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
            //GetDbContext().Database.EnsureCreated();
            _provider = services.BuildServiceProvider();
            _scopeFactory = _provider.GetService<IServiceScopeFactory>();

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ConduitContext>().Database.EnsureCreated();
            }
            await Task.CompletedTask;
        }
        public SliceFixture()
        {
            //_provider = services.BuildServiceProvider();
            //_scopeFactory = _provider.GetService<IServiceScopeFactory>();
        }

        public ConduitContext GetDbContext() => _provider.GetRequiredService<ConduitContext>();

        public async Task DeleteDatabaseAsync()
        {
            File.Delete(_dbName);
            await Task.CompletedTask;
        }
        public async Task DisposeAsync() => await DeleteDatabaseAsync();

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                return await action(scope.ServiceProvider);
            }
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) => ExecuteScopeAsync(sp =>
                                                                                             {
                                                                                                 var mediator = sp.GetService<IMediator>();

                                                                                                 return mediator.Send(request);
                                                                                             });

        public Task SendAsync(IRequest request) => ExecuteScopeAsync(sp =>
                                                            {
                                                                var mediator = sp.GetService<IMediator>();

                                                                return mediator.Send(request);
                                                            });

        public Task ExecuteDbContextAsync(Func<ConduitContext, Task> action) => ExecuteScopeAsync(sp => action(sp.GetService<ConduitContext>()));

        public Task<T> ExecuteDbContextAsync<T>(Func<ConduitContext, Task<T>> action) => ExecuteScopeAsync(sp => action(sp.GetService<ConduitContext>()));

        public Task InsertAsync(params object[] entities) => ExecuteDbContextAsync(db =>
                                                                      {
                                                                          foreach (var entity in entities)
                                                                          {
                                                                              db.Add(entity);
                                                                          }
                                                                          return db.SaveChangesAsync();
                                                                      });
    }
}
