using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Entities;

namespace MedSolutions.Domain.Entities;

public class AppointmentType : BusinessEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Enums.AppointmentType Id { get; set; }
    [ForeignKey(nameof(MedicalSpecialty))]
    public Enums.MedicalSpecialty MedicalSpecialtyId { get; set; }
    public MedicalSpecialty? MedicalSpecialty { get; set; }
    [MaxLength(50)]
    public string Description { get; set; } = default!;
}


