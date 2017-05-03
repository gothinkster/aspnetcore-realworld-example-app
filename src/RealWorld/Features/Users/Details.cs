using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Users
{
    public class Details
    {
        public class Query : IRequest<UserEnvelope>
        {
            public string Username { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, UserEnvelope>
        {
            private readonly RealWorldContext _context;

            public QueryHandler(RealWorldContext context)
            {
                _context = context;
            }

            public async Task<UserEnvelope> Handle(Query message)
            {
                var person = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.Username);
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                return new UserEnvelope(Mapper.Map<Domain.Person, User>(person));
            }
        }
    }
}