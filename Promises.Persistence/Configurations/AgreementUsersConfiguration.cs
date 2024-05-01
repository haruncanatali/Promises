using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promises.Domain.Entities;

namespace Promises.Persistence.Configurations;

public class AgreementUsersConfiguration : IEntityTypeConfiguration<AgreementUsers>
{
    public void Configure(EntityTypeBuilder<AgreementUsers> builder)
    {
        builder.Property(c => c.PromiserUserId).IsRequired();
        builder.Property(c => c.PromisedUserId).IsRequired();
        builder.Property(c => c.AgreementId).IsRequired();
    }
}