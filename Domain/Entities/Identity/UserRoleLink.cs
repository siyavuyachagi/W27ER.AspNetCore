using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    [Table("AspNetUserRoleLinks")]
    public class UserRoleLink: IdentityUserRole<Guid>
    {
    }
}
