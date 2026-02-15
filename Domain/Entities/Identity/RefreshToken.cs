using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    [Table("AspNetRefreshTokens")]
    public class RefreshToken
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public string EncodedToken { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime RevokedAt { get; set; }

        public string? Device { get; set; }
        public string? IpAddress { get; set; }
        public string? Location { get; set; }

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
