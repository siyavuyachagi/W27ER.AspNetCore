using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    [Table("AspNetUserRoleLinks")]
    public class UserRoleLink: IdentityUserRole<Guid>
    {
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; } = default!;

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; } = default!;
    }
}
