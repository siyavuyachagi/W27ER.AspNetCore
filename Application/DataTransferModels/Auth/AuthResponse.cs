namespace Application.DataTransferModels.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public AuthUser User { get; set; }
}
