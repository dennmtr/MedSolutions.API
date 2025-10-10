using System.ComponentModel.DataAnnotations;
using MedSolutions.Domain.Common.Models;

namespace MedSolutions.Domain.Models;

public class PatientPairType : BusinessEntity
{
    [Key]
    public short Id { get; set; }
    [MaxLength(50)]
    public string Description { get; set; } = default!;
}


