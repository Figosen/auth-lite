namespace Api.DTO;

public class LoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; } //Todo: Få krav på
    public string? Nonce { get; set; } //Todo: Få krav på
}