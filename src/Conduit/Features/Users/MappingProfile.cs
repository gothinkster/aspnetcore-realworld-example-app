using AutoMapper;

namespace Conduit.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Person, User>(MemberList.None);
        }
    }
}
