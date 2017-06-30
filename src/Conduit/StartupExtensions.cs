using System;
using System.Text;
using System.Threading.Tasks;
using Conduit.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Conduit
{
    public static class StartupExtensions
    {
        public static void AddJwt(this IServiceCollection services)
        {
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("somethinglongerforthisdumbalgorithmisrequired"));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = "issuer";
                options.Audience = "Audience";
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        public static void UseJwt(this IApplicationBuilder app)
        {
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
        }

        public static void AddSerilogLogging(this ILoggerFactory loggerFactory)
        {
            // Attach the sink to the logger configuration
            var log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                //just for local debug
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            loggerFactory.AddSerilog(log);
            Log.Logger = log;
        }
    }
}