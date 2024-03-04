using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Domain;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using Conduit.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Users;

public class Create
{
    public record UserData(string? Username, string? Email, string? Password);

    public record Command(UserData User) : IRequest<UserEnvelope>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.User.Username).NotNull().NotEmpty();
            RuleFor(x => x.User.Email).NotNull().NotEmpty();
            RuleFor(x => x.User.Password).NotNull().NotEmpty();
        }
    }

    public class Handler(
        ConduitContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper
    ) : IRequestHandler<Command, UserEnvelope>
    {
        public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
        {
            if (
                await context
                    .Persons.Where(x => x.Username == message.User.Username)
                    .AnyAsync(cancellationToken)
            )
            {
                throw new RestException(
                    HttpStatusCode.BadRequest,
                    new { Username = Constants.IN_USE }
                );
            }

            if (
                await context
                    .Persons.Where(x => x.Email == message.User.Email)
                    .AnyAsync(cancellationToken)
            )
            {
                throw new RestException(
                    HttpStatusCode.BadRequest,
                    new { Email = Constants.IN_USE }
                );
            }

            var salt = Guid.NewGuid().ToByteArray();
            var person = new Person
            {
                Username = message.User.Username,
                Email = message.User.Email,
                Hash = await passwordHasher.Hash(
                    message.User.Password ?? throw new InvalidOperationException(),
                    salt
                ),
                Salt = salt
            };

            await context.Persons.AddAsync(person, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var user = mapper.Map<Person, User>(person);
            user.Token = jwtTokenGenerator.CreateToken(
                person.Username ?? throw new InvalidOperationException()
            );
            return new UserEnvelope(user);
        }
    }
}
