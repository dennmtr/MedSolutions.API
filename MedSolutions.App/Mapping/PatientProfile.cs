using AutoMapper;
using MedSolutions.App.ViewModels;
using MedSolutions.Domain.Models;

namespace MedSolutions.App.Mapping;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<Patient, PatientViewModel>()
           ;
    }
}
