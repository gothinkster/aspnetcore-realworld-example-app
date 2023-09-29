using System;
using System.IO;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.IntegrationTests
{
    public class SliceFixture : IDisposable
    {
        //private static readonly IConfiguration CONFIG;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ServiceProvider _provider;
        private readonly string _dbName = Guid.NewGuid() + ".db";

        //static SliceFixture() => CONFIG = new ConfigurationBuilder()
        //       .AddEnvironmentVariables()
        //       .Build();

        public SliceFixture()
        {
            //var startup = new Startup();
            var services = new ServiceCollection();

            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(_dbName);
            services.AddSingleton(new ConduitContext(builder.Options));

            //startup.ConfigureServices(services);

            _provider = services.BuildServiceProvider();

            GetDbContext().Database.EnsureCreated();
            _scopeFactory = _provider.GetService<IServiceScopeFactory>();
        }

        public ConduitContext GetDbContext() => _provider.GetRequiredService<ConduitContext>();

        public void Dispose() => File.Delete(_dbName);

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
