using System;
using System.Linq;
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
    public class Login
    {
        public class UserData
        {
            public string? Email { get; set; }

            public string? Password { get; set; }
        }

        public record Command(UserData User) : IRequest<UserEnvelope>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull();
                RuleFor(x => x.User.Email).NotNull().NotEmpty();
                RuleFor(x => x.User.Password).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public Handler(ConduitContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }

            public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var person = await _context.Persons.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync(cancellationToken);
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                if (!person.Hash.SequenceEqual(await _passwordHasher.Hash(message.User.Password ?? throw new InvalidOperationException(), person.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                var user = _mapper.Map<Domain.Person, User>(person);
                user.Token = _jwtTokenGenerator.CreateToken(person.Username ?? throw new InvalidOperationException());
                return new UserEnvelope(user);
            }
        }
    }
}
