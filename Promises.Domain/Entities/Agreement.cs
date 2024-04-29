using Promises.Domain.Base;
using Promises.Domain.Enums;
using Promises.Domain.Identity;

namespace Promises.Domain.Entities;

public class Agreement : BaseEntity
{
    public string Description { get; set; }
    public PriorityLevel PriorityLevel { get; set; }
    public DateTime Date { get; set; }
    public CommitmentStatus CommitmentStatus { get; set; }
    public bool HasNotification { get; set; }
    public bool HasMailNotification { get; set; }
    public int NotificationFrequency { get; set; }

    public long PersonId { get; set; }
    public long UserId { get; set; }

    public Person Person { get; set; }
    public User User { get; set; }
    public List<EventPhoto> EventPhotos { get; set; }
}