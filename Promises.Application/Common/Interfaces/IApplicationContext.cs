using Microsoft.EntityFrameworkCore;
using Promises.Domain.Entities;
using Promises.Domain.Identity;

namespace Promises.Application.Common.Interfaces;

public interface IApplicationContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Agreement> Agreements { get; set; }
    public DbSet<EventPhoto> EventPhotos { get; set; }
    public DbSet<AgreementUsers> AgreementUsers { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<BlockedFriends> BlockedFriends { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}