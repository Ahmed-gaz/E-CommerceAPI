namespace E_CommerceAPI.DTOs
{
    public record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}
