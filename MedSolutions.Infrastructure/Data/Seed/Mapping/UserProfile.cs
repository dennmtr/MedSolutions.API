using AutoMapper;
using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Seed.DTOs;

namespace MedSolutions.Infrastructure.Data.Seed.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserConfigureDTO, User>()
            .ForMember(u => u.UserName, s => s.MapFrom(u => u.Email))
        ;
    }
}
