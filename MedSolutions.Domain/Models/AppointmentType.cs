using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Models;

namespace MedSolutions.Domain.Models;

public class AppointmentType : BusinessEntity
{
    [Key]
    public short Id { get; set; }
    [ForeignKey(nameof(MedicalSpecialty))]
    public Enums.MedicalSpecialty MedicalSpecialtyId { get; set; }
    public MedicalSpecialty? MedicalSpecialty { get; set; }
    [MaxLength(50)]
    public string Description { get; set; } = default!;
}


