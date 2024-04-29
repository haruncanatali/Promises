using Microsoft.AspNetCore.Identity;

namespace Promises.Domain.Identity;

public class UserRole : IdentityUserRole<long>
{
    public UserRole() : base()
    {
            
    }
}