using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class Role: IdentityRole<Guid>
    {
        // Navigation property
        //public ICollection<UserRoleLink> UserRoleLinks { get; set; } = [];
    }
}
