using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string? ProfileImageUrl { get; set; }

        public Guid? PhysicalAddressId { get; set; }
        [ForeignKey(nameof(PhysicalAddressId))]
        public PhysicalAddress? PhysicalAddress { get; set; }

        // Navigations
        public ICollection<UserRoleLink> UserRoleLinks { get; set; } = [];
    }
}
