using AutoMapper;

namespace RealWorld.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Person, Domain.User>(MemberList.None);
        }
    }
}
