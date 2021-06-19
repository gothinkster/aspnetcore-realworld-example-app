using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Tags
{
    public class List
    {
        public record Query : IRequest<TagsEnvelope>;

        public class QueryHandler : IRequestHandler<Query, TagsEnvelope>
        {
            private readonly ConduitContext _context;

            public QueryHandler(ConduitContext context)
            {
                _context = context;
            }

            public async Task<TagsEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var tags = await _context.Tags.OrderBy(x => x.TagId).AsNoTracking().ToListAsync(cancellationToken);
                return new TagsEnvelope()
                {
                    Tags = tags?.Select(x => x.TagId ?? string.Empty).ToList() ?? new List<string>()
                };
            }
        }
    }
}
