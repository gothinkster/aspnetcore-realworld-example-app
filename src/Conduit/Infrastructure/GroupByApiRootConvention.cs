using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Conduit.Infrastructure
{
    public class GroupByApiRootConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.Attributes.OfType<RouteAttribute>().FirstOrDefault();
            var apiVersion = controllerNamespace?.Template?.Split('/').First().ToLowerInvariant() ?? "default";

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
