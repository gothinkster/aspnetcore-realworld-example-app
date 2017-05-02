using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RealWorld.Domain;

namespace RealWorld.Infrastructure
{
    public class EntityModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return typeof(IEntity).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
        }
    }
}