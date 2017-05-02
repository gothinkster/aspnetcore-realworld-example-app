using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealWorld;
using RealWorld.Domain;
using RealWorld.Infrastructure;

namespace Realworld.IntegrationTests
{
    public class SliceFixture : IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly string DbName = Guid.NewGuid() + ".db";

        public SliceFixture()
        {
            var startup = new Startup();
            var services = new ServiceCollection();

            services.AddSingleton(new RealWorldContext(DbName));

            startup.ConfigureServices(services);

            var provider = services.BuildServiceProvider();


            provider.GetRequiredService<RealWorldContext>().Database.EnsureCreated();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();
        }

        public void Dispose()
        {
            File.Delete(DbName);
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
        
        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task ExecuteDbContextAsync(Func<RealWorldContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<RealWorldContext>()));
        }

        public Task<T> ExecuteDbContextAsync<T>(Func<RealWorldContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<RealWorldContext>()));
        }

        public Task InsertAsync(params object[] entities)
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }
    }
}