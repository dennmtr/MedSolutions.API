using System.ComponentModel.DataAnnotations;

namespace MedSolutions.Domain.Common.Models;

public class BusinessEntity : BaseEntity
{
    public int DisplayOrder { get; set; }

    [StringLength(100, MinimumLength = 5)]
    public string BusinessId { get; set; } = default!;
}
