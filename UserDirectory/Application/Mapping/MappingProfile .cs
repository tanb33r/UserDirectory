using AutoMapper;
using UserDirectory.Domain;
using UserDirectory.Application.Dtos;

namespace UserDirectory.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Role, RoleDto>();

        CreateMap<Contact, ContactDto>();

        CreateMap<User, UserDto>()
            .ForMember(d => d.Sex,
                       o => o.MapFrom(s => s.Sex == Sex.M ? "M" : "F"));


        CreateMap<CreateContactDto, Contact>();

        CreateMap<CreateUserDto, User>()
            .ForMember(d => d.Sex,
                       o => o.MapFrom(s => s.Sex == "M" ? Sex.M : Sex.F));

        CreateMap<UpdateUserDto, User>()
            .ForMember(d => d.Sex,
                       o => o.MapFrom(s => s.Sex == "M" ? Sex.M : Sex.F))
            .ForMember(d => d.Contact, o => o.MapFrom(s => s.Contact))
            .ForAllMembers(o => o.Condition((src, dest, val) => val is not null));
    }
}
