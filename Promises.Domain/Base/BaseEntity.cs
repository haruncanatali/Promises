namespace Promises.Domain.Base;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }

    public BaseEntity()
    {
        CreatedAt = DateTime.Now;
    }
}