using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promises.Domain.Entities;

namespace Promises.Persistence.Configurations;

public class EventPhotoConfiguration : IEntityTypeConfiguration<EventPhoto>
{
    public void Configure(EntityTypeBuilder<EventPhoto> builder)
    {
        builder.Property(c => c.Photo).IsRequired();
        builder.Property(c => c.AgreementId).IsRequired();
    }
}