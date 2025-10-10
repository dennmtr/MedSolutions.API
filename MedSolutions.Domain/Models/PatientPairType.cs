using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Models;

namespace MedSolutions.Domain.Models;

public class PatientPairType : BusinessEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public short Id { get; set; }
    [MaxLength(50)]
    public string Description { get; set; } = default!;
}


