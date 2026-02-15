using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class Role: IdentityRole<Guid>
    {
        public ICollection<UserRoleLink> UserRoleLinks { get; set; } = [];
    }
}
