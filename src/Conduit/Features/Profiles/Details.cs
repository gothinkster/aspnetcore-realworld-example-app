using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Conduit.Features.Profiles
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

        public class QueryHandler : IRequestHandler<Query, ProfileEnvelope>
        {
            private readonly IProfileReader _profileReader;

            public QueryHandler(IProfileReader profileReader)
            {
                _profileReader = profileReader;
            }

            public async Task<ProfileEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                return await _profileReader.ReadProfile(message.Username);
            }
        }
    }
}