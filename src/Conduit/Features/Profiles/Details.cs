using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Conduit.Features.Profiles;

public class Details
{
    public record Query(string Username) : IRequest<ProfileEnvelope>;

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => RuleFor(x => x.Username).NotEmpty();
    }

    public class QueryHandler(IProfileReader profileReader)
        : IRequestHandler<Query, ProfileEnvelope>
    {
        public Task<ProfileEnvelope> Handle(Query message, CancellationToken cancellationToken) =>
            profileReader.ReadProfile(message.Username, cancellationToken);
    }
}
