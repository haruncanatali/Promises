namespace Promises.Application.Common.Models;

public class BaseDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
}