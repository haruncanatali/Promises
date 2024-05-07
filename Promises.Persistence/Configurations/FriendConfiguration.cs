using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promises.Domain.Entities;

namespace Promises.Persistence.Configurations;

public class FriendConfiguration : IEntityTypeConfiguration<Friend>
{
    public void Configure(EntityTypeBuilder<Friend> builder)
    {
        builder.Property(c => c.SenderId).IsRequired();
        builder.Property(c => c.ReceiverId).IsRequired();
        builder.Property(c => c.Approved).HasDefaultValue(false).IsRequired();
    }
}