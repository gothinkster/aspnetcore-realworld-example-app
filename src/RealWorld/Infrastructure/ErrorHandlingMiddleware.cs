using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RealWorld.Infrastructure
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            if (exception is RestException re)
            {
                code = re.Code;
            }


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                var result = JsonConvert.SerializeObject(new {error = exception.Message});
                await context.Response.WriteAsync(result);
            }
        }
    }
}