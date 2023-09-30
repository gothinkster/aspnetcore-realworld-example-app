using System;
using System.Threading.Tasks;
using Conduit.Features.Profiles;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Security;
using FluentValidation.AspNetCore;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Http.Json;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.IO;

namespace Conduit.IntegrationTests
{
    [CollectionDefinition(nameof(SliceFixture))]
    public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }
    public partial class SliceFixture : IAsyncLifetime, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _dbName = Guid.NewGuid() + ".db";
        public async Task InitializeAsync() => await Task.CompletedTask;
        public SliceFixture()
        {
            _factory = new ConduitTestApplicationFactory(_dbName);
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ConduitContext>().Database.EnsureCreated();
            }
        }

        public sealed class ConduitTestApplicationFactory
        : WebApplicationFactory<Program>
        {
            public string database
            {
                get; set;
            }

            public ConduitTestApplicationFactory(string database) => this.database = database;

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                var services = new ServiceCollection();

                services.AddEndpointsApiExplorer();
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DBContextTransactionPipelineBehavior<,>));
                //services.AddDbContext<ConduitContext>(options => options.UseInMemoryDatabase(_dbName));
                var dbBuilder = new DbContextOptionsBuilder();
                dbBuilder.UseInMemoryDatabase(database);
                //dbBuilder.UseSqlite(database);
                services.AddSingleton(new ConduitContext(dbBuilder.Options));
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
                //_provider = services.BuildServiceProvider();
                //_scopeFactory = _provider.GetService<IServiceScopeFactory>();     
            }
        }
        public ConduitContext GetDbContext()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<ConduitContext>();
            }
        }



        //public async Task DeleteDatabaseAsync()
        //{
        //    File.Delete(_dbName);
        //    await Task.CompletedTask;
        //}
        public Task DisposeAsync()
        {
            _factory?.Dispose();
            return Task.CompletedTask;
        }

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

        //public Task<TResponse> SendHandlerAsync<TResponse>(IRequest<TResponse> request) => ExecuteScopeAsync(sp =>
        //{
        //    var mediator = sp.GetService<IMediator>();
        //    var context = sp.GetService<ConduitContext>();
        //    var currentUserAccessor = sp.GetService<ICurr>();
        //    return mediator.Send(request);
        //});

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

        public void Dispose() =>
            File.Delete(_dbName);
    }
}
