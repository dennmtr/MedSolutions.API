using System.ComponentModel.DataAnnotations;

namespace MedSolutions.App.ViewModels;
public class PatientViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Patronymic { get; set; }
    public string? AMKA { get; set; }
    public string? PersonalIdNumber { get; set; }
    public Domain.Enums.Gender Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Address { get; set; }
    public string City { get; set; } = default!;
    public string? Zip { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public bool? Biopsy { get; set; } = false;
    [DisplayFormat(DataFormatString = "{0:F6}")]
    public decimal? Latitude { get; set; }

    [DisplayFormat(DataFormatString = "{0:F6}")]
    public decimal? Longitude { get; set; }
    public DateTime? MostRecentAppointment { get; set; }
    public DateTime? NextScheduledAppointment { get; set; }
    public int AppointmentHistoryCount { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }

}
