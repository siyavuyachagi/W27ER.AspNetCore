using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferModels;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}
