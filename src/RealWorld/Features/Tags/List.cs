using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Tags
{
    public class List
    {
        public class Query : IRequest<TagsEnvelope>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, TagsEnvelope>
        {
            private readonly RealWorldContext _context;

            public QueryHandler(RealWorldContext context)
            {
                _context = context;
            }

            public async Task<TagsEnvelope> Handle(Query message)
            {
                var tags = await _context.Tags.OrderBy(x => x.TagId).AsNoTracking().ToListAsync();
                return new TagsEnvelope()
                {
                    Tags = tags.Select(x => x.TagId).ToList()
                };
            }
        }
    }
}