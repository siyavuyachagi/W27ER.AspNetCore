using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    [Table("AspNetUserRoleLinks")]
    public class UserRoleLink: IdentityUserRole<Guid>
    {
        // Navigations
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public Role Role { get; set; } = default!;
    }
}
