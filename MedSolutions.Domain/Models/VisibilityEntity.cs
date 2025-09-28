namespace MedSolutions.Domain.Models;

public class VisibilityEntity : BaseEntity
{
    public bool? IsHidden { get; set; } = false;
}
