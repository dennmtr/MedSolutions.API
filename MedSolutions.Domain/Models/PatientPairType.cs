using System.ComponentModel.DataAnnotations;

namespace MedSolutions.Domain.Models;

public class PatientPairType : BusinessEntity
{
    [Key]
    public short Id { get; set; }
    [MaxLength(50)]
    public string Description { get; set; } = null!;
}


