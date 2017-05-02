using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Users
{
    public class Details
    {
        public class Query : IRequest<Domain.User>
        {
            public string Username { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Domain.User>
        {
            private readonly RealWorldContext _context;

            public QueryHandler(RealWorldContext context)
            {
                _context = context;
            }

            public async Task<Domain.User> Handle(Query message)
            {
                var person = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.Username);
                return Mapper.Map<Domain.Person, Domain.User>(person);
            }
        }
    }
}