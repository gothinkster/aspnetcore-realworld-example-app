using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly ConduitContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public ProfileReader(ConduitContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<ProfileEnvelope> ReadProfile(string username, CancellationToken cancellationToken)
        {
            var currentUserName = _currentUserAccessor.GetCurrentUsername();

            var person = await _context.Persons.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

            if (person == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });
            }
            var profile = _mapper.Map<Domain.Person, Profile>(person);

            if (currentUserName != null)
            {
                var currentPerson = await _context.Persons
                    .Include(x => x.Following)
                    .Include(x => x.Followers)
                    .FirstOrDefaultAsync(x => x.Username == currentUserName, cancellationToken);

                if (currentPerson.Followers.Any(x => x.TargetId == person.PersonId))
                {
                    profile.IsFollowed = true;
                }
            }

            return new ProfileEnvelope(profile);
        }
    }
}
