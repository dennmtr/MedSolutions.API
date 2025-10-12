namespace MedSolutions.Domain.Common.Entities;

public abstract class BaseEntity
{
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public bool IsDeleted { get; set; }
}
