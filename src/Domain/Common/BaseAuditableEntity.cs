namespace GerPros_Backend_API.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity<Guid>
{
    protected BaseAuditableEntity()
    {
        Id = Guid.NewGuid();
    }
    
    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
