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
public class DBContextTransactionPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ConduitContext _context;

    public DBContextTransactionPipelineBehavior(ConduitContext context) => _context = context;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        TResponse? result = default;

        try
        {
            _context.BeginTransaction();

            result = await next();

            _context.CommitTransaction();
        }
        catch (Exception)
        {
            _context.RollbackTransaction();
            throw;
        }

        return result;
    }
}