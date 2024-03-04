using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Conduit.Infrastructure;

/// <summary>
/// Adds transaction to the processing pipeline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class DBContextTransactionPipelineBehavior<TRequest, TResponse>(ConduitContext context)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        TResponse? result;

        try
        {
            context.BeginTransaction();

            result = await next();

            context.CommitTransaction();
        }
        catch (Exception)
        {
            context.RollbackTransaction();
            throw;
        }

        return result;
    }
}
