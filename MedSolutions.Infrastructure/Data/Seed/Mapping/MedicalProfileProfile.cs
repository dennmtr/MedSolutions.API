using AutoMapper;
using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Seed.DTOs;
using MedSolutions.Shared.Extensions;

namespace MedSolutions.Infrastructure.Data.Seed.Mapping;

public class MedicalProfileProfile : Profile
{
    public MedicalProfileProfile()
    {
        CreateMap<MedicalProfileConfigureDTO, MedicalProfile>()
            .ForMember(p => p.SubscriptionStartDate, s => s.MapFrom(p => DateTime.UtcNow.TrimToSeconds()))
            .ForMember(p => p.SubscriptionEndDate, s => s.MapFrom(p => DateTime.UtcNow.AddYears(1).TrimToSeconds()))
        ;
    }
}
