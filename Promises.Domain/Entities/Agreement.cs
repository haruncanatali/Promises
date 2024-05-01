using Promises.Domain.Base;
using Promises.Domain.Enums;
using Promises.Domain.Identity;

namespace Promises.Domain.Entities;

public class Agreement : BaseEntity
{
    public string Description { get; set; }
    public PriorityLevel PriorityLevel { get; set; }
    public DateTime Date { get; set; }
    public bool HasNotification { get; set; }
    public bool HasMailNotification { get; set; }
    public NotificationFrequency NotificationFrequency { get; set; }
    public bool Approved { get; set; }

    public List<AgreementUsers> AgreementUsers { get; set; }
    public List<EventPhoto> EventPhotos { get; set; }
}