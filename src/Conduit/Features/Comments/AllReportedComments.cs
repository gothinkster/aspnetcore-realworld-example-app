using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Features.Comments
{
    public class AllReportedComments
    {
        public class Query : IRequest<CommentsEnvelope>
        {
            public Query()
            {

            }
        }

        public class QueryHandler : IRequestHandler<Query, CommentsEnvelope>
        {
            private readonly ConduitContext _context;

            public QueryHandler(ConduitContext context)
            {
                _context = context;
            }

            public async Task<CommentsEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var reportedComments = await _context.Comments
                    .Where(x => x.IsReported == true)
                    .AsNoTracking()
                    .ToListAsync();

                if (reportedComments == null || reportedComments.Count == 0)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
                }

                return new CommentsEnvelope()
                {
                    Comments = reportedComments,
                    CommentsCount = reportedComments.Count()
                };
            }
        }
    }
}
