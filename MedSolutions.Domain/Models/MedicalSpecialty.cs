using System.ComponentModel.DataAnnotations;
using MedSolutions.Domain.Common.Models;

namespace MedSolutions.Domain.Models;

public class MedicalSpecialty : BusinessEntity
{
    [Key]
    public Enums.MedicalSpecialty Id { get; set; }

    [MaxLength(50)]
    public string Description { get; set; } = null!;
}
