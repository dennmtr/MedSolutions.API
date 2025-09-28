using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedSolutions.Domain.Models;

public class MedicalProfile : BaseEntity
{
    [Key]
    [ForeignKey(nameof(User))]
    public string Id { get; set; } = null!;

    public User? User { get; set; }

    [MaxLength(50)]
    public string CompanyName { get; set; } = null!;

    [MaxLength(50)]
    public string Tin { get; set; } = null!;

    [MaxLength(100)]
    public string Address { get; set; } = null!;

    [MaxLength(50)]
    public string City { get; set; } = null!;

    [MaxLength(50)]
    public string Zip { get; set; } = null!;
    public string? Comments { get; set; }
    public DateTime SubscriptionStartDate { get; set; }
    public DateTime SubscriptionEndDate { get; set; }

    [ForeignKey(nameof(MedicalSpecialty))]
    public Enums.MedicalSpecialty? MedicalSpecialtyId { get; set; } = Enums.MedicalSpecialty.Unspecified;
    public MedicalSpecialty? MedicalSpecialty { get; set; }
    public ICollection<Patient> Patients { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];

}
