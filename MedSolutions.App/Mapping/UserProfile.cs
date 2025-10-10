using AutoMapper;
using MedSolutions.App.Common.DTOs;
using MedSolutions.App.DTOs;
using MedSolutions.Domain.Entities;

namespace MedSolutions.App.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, LoginResponseDTO>()
           ;
        CreateMap<User, UserDTO>()
           ;
    }
}
