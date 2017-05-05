using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using RealWorld.Features.Profiles;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Followers
{
    public class Add
    {
        public class Command : IRequest<ProfileEnvelope>
        {
            public Command(string username)
            {
                Username = username;
            }

            public string Username { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                DefaultValidatorExtensions.NotNull(RuleFor(x => x.Username)).NotEmpty();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Command, ProfileEnvelope>
        {
            private readonly RealWorldContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IProfileReader _profileReader;

            public QueryHandler(RealWorldContext context, ICurrentUserAccessor currentUserAccessor, IProfileReader profileReader)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
                _profileReader = profileReader;
            }

            public async Task<ProfileEnvelope> Handle(Command message)
            {
                var target = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.Username);

                if (target == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                
                var observer = await _context.Persons.FirstOrDefaultAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername());

                var followedPeople = await _context.FollowedPeople.FirstOrDefaultAsync(x => x.ObserverId == observer.PersonId && x.TargetId == target.PersonId);

                if (followedPeople == null)
                {
                    followedPeople = new FollowedPeople()
                    {
                        Observer = observer,
                        ObserverId = observer.PersonId,
                        Target = target,
                        TargetId = target.PersonId
                    };
                    await _context.FollowedPeople.AddAsync(followedPeople);
                    await _context.SaveChangesAsync();
                }

                return await _profileReader.ReadProfile(message.Username);
            }
        }
    }
}