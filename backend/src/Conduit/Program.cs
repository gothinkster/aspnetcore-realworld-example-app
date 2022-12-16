using Conduit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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

await host.RunAsync();
