using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using RealWorld.Features.Followers;

namespace RealWorld.Features.Profiles
{
    public class Details
    {
        public class Query : IRequest<ProfileEnvelope>
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

        public class QueryHandler : IAsyncRequestHandler<Query, ProfileEnvelope>
        {
            private readonly IProfileReader _profileReader;

            public QueryHandler(IProfileReader profileReader)
            {
                _profileReader = profileReader;
            }

            public async Task<ProfileEnvelope> Handle(Query message)
            {
                return await _profileReader.ReadProfile(message.Username);
            }
        }
    }
}