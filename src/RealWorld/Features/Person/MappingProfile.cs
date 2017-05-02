using AutoMapper;

namespace RealWorld.Features.Person
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Domain.Person>(MemberList.Source);
        }
    }
}
