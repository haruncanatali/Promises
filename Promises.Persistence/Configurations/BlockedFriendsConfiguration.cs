using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promises.Domain.Entities;

namespace Promises.Persistence.Configurations
{
    public class BlockedFriendsConfiguration : IEntityTypeConfiguration<BlockedFriends>
    {
        public void Configure(EntityTypeBuilder<BlockedFriends> builder)
        {
            builder.Property(c => c.ReceiverId).IsRequired();
            builder.Property(c => c.SenderId).IsRequired();
        }
    }
}
