using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Conduit.Infrastructure
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationPipelineBehavior(IValidator<TRequest> validators)
        {
            _validator = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var result = await _validator.ValidateAsync(context, cancellationToken);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return await next();
        }
    }
}