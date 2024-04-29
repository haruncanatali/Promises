using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promises.Domain.Entities;

namespace Promises.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(c=>c.Name).IsRequired();
        builder.Property(c=>c.Surname).IsRequired();
        builder.Property(c=>c.Age).IsRequired();
        builder.Property(c=>c.Photo).IsRequired();
    }
}