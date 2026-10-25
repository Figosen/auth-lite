namespace Api.DTO;

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; } //Todo: Få krav på
    public string? Nonce { get; set; } //Todo: Få krav på
}