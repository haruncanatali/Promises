using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Persistence.Configurations;

public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
{
    public void Configure(EntityTypeBuilder<Agreement> builder)
    {
        builder.Property(c => c.Title).IsRequired();
        builder.Property(c => c.Description).IsRequired();
        builder.Property(c => c.PriorityLevel).IsRequired();
        builder.Property(c => c.Date).IsRequired();
        builder.Property(c => c.HasNotification).IsRequired();
        builder.Property(c => c.HasMailNotification).IsRequired();
        builder.Property(c => c.NotificationFrequency).IsRequired();
        builder.Property(c => c.Approved).HasDefaultValue(false).IsRequired();
        builder.Property(c => c.EntityStatus).HasDefaultValue(EntityStatus.Waiting).IsRequired();
    }
}