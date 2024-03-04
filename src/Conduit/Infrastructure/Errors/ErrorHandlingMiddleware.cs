using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Errors;

public class ErrorHandlingMiddleware(
    RequestDelegate next,
    IStringLocalizer<ErrorHandlingMiddleware> localizer,
    ILogger<ErrorHandlingMiddleware> logger
)
{
    private static readonly Action<ILogger, string, Exception> LOGGER_MESSAGE =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            eventId: new EventId(id: 0, name: "ERROR"),
            formatString: "{Message}"
        );

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger, localizer);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        ILogger<ErrorHandlingMiddleware> logger,
        IStringLocalizer<ErrorHandlingMiddleware> localizer
    )
    {
        string? result;
        switch (exception)
        {
            case RestException re:
                context.Response.StatusCode = (int)re.Code;
                result = JsonSerializer.Serialize(new { errors = re.Errors });
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                LOGGER_MESSAGE(logger, "Unhandled Exception", exception);
                result = JsonSerializer.Serialize(
                    new { errors = localizer[Constants.InternalServerError].Value }
                );
                break;
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}
