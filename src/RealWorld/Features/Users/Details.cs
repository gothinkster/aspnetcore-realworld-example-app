using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Users
{
    public class Details
    {
        public class Query : IRequest<UserEnvelope>
        {
            public string Username { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
            }
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
                var person = await _context.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == message.Username);
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                return new UserEnvelope(Mapper.Map<Domain.Person, User>(person));
            }
        }
    }
}