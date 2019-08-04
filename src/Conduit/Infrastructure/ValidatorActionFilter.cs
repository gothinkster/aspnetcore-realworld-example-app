using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Conduit.Infrastructure
{
    public class ValidatorActionFilter : IActionFilter
    {
        public ValidatorActionFilter(ILogger<ValidatorActionFilter> logger)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ModelState.IsValid)
            {
                return;
            }

            var result = new ContentResult();
            var errors = new Dictionary<string, string[]>();

            foreach (var (key, value) in filterContext.ModelState)
            {
                errors.Add(key, value.Errors.Select(x => x.ErrorMessage).ToArray());
            }

            var content = JsonConvert.SerializeObject(new { errors });
            result.Content = content;
            result.ContentType = "application/json";

            filterContext.HttpContext.Response.StatusCode = 422; //unprocessable entity;
            filterContext.Result = result;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
