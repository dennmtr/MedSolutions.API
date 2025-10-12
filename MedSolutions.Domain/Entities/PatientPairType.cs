using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Entities;

namespace MedSolutions.Domain.Entities;

public class PatientPairType : BusinessEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Enums.PatientPairType Id { get; set; }
    [MaxLength(50)]
    public string Description { get; set; } = default!;

}


