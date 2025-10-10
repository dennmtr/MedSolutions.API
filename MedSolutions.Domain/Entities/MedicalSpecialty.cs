using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Models;

namespace MedSolutions.Domain.Entities;

public class MedicalSpecialty : BusinessEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Enums.MedicalSpecialty Id { get; set; }

    [MaxLength(50)]
    public string Description { get; set; } = default!;
}
