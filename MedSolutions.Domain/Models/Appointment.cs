using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Models;

namespace MedSolutions.Domain.Models;

public class Appointment : VisibilityEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(MedicalProfile))]
    public Guid MedicalProfileId { get; set; } = default!;
    public MedicalProfile? MedicalProfile { get; set; }

    [ForeignKey(nameof(Patient))]
    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }
    public DateTime Date { get; set; }

    [ForeignKey(nameof(AppointmentType))]
    public short AppointmentTypeId { get; set; }
    public AppointmentType? AppointmentType { get; set; }
    public string? Comments { get; set; }
    public Enums.State? State { get; set; }
    public Enums.Priority? Priority { get; set; }


}


