using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Conduit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // read database configuration (database provider + database connection) from environment variables
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseUrls($"http://+:5000")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
