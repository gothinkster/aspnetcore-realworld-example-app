using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Conduit.Infrastructure.Errors
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IStringLocalizer<ErrorHandlingMiddleware> _localizer;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            IStringLocalizer<ErrorHandlingMiddleware> localizer,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this._logger = logger;
            this._localizer = localizer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger, _localizer);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger<ErrorHandlingMiddleware> logger,
            IStringLocalizer<ErrorHandlingMiddleware> localizer)
        {
            if (exception is RestException re)
            {
                context.Response.StatusCode = (int)re.Code;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(exception.Message))
                {
                    logger.LogError("", exception.Message);
                }

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new
                {
                    errors = localizer[Constants.ErrorHandlingMiddleware.InternalServerError].Value
                });
                await context.Response.WriteAsync(result);
            }

        }
    }
}