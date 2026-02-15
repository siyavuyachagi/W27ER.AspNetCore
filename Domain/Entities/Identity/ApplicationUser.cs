using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
