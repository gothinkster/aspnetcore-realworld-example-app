using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using Conduit.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Users
{
    public class Details
    {
        public record Query(string Username) : IRequest<UserEnvelope>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, UserEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public QueryHandler(ConduitContext context, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = context;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }

            public async Task<UserEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var person = await _context.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == message.Username, cancellationToken);

                if (person == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });
                }

                var user = _mapper.Map<Domain.Person, User>(person);
                user.Token = _jwtTokenGenerator.CreateToken(person.Username ?? throw new InvalidOperationException());
                return new UserEnvelope(user);
            }
        }
    }
}
