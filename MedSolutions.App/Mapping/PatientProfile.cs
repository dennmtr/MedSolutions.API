using AutoMapper;
using MedSolutions.App.ViewModels;
using MedSolutions.Domain.Entities;

namespace MedSolutions.App.Mapping;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<Patient, PatientViewModel>()
            .ForMember(p => p.MostRecentAppointment, opt => opt.MapFrom(p =>
                p.Appointments
                 .Where(a => a.Date < DateTime.UtcNow)
                 .Max(a => (DateTime?)a.Date)
            ))
            .ForMember(p => p.NextScheduledAppointment, opt => opt.MapFrom(p =>
                p.Appointments
                 .Where(a => a.Date >= DateTime.UtcNow)
                 .Min(a => (DateTime?)a.Date)
            ))
            .ForMember(p => p.AppointmentHistoryCount, opt => opt.MapFrom(p =>
                p.Appointments.Count()
            ));
    }
}
