using Domain.Entities;
using Domain.Entities.Identity;

namespace Application.DataTransferModels.Auth;

public class AuthUser
{
    public AuthUser(ApplicationUser user, List<string> roles)
    {
        this.Id = user.Id;
        this.Email = user.Email;
        this.UserName = user.UserName;
        this.FirstName = user.FirstName;
        this.MiddleName = user.MiddleName;
        this.LastName = user.LastName;
        this.Avatar = user.ProfileImageUrl;
        this.Roles = roles;
    }

    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = default!;
    public string? Avatar { get; set; }
    public List<string> Roles { get; set; } = [];
}
