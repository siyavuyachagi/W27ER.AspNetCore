using Domain.Entities.Identity;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Employer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // BASIC INFORMATION
        public string Name { get; set; } = default!;

        public EmployerTypeCode EmployerType { get; set; }

        [MaxLength(50)]
        public string? RegistrationNumber { get; set; }

        public string? Description { get; set; }
        public string? ContactPersonName { get; set; }
        public string ContactEmail { get; set; } = default!;
        public string ContactPhone { get; set; } = default!;

        public Guid PhysicalAddressId { get; set; }
        [ForeignKey(nameof(PhysicalAddressId))]
        public PhysicalAddress? PhysicalAddress { get; set; }


        // USER ACCOUNT LINK (Optional - for employer portal access)
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }

        // STATUS & AUDIT
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // NAVIGATION PROPERTIES
        //public virtual ICollection<WorkOpportunity> WorkOpportunities { get; set; } = new List<WorkOpportunity>();
    }
}
