namespace Api.Models.EF;

public class Auth
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? Name { get; set; }
    public string? Picture { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

}