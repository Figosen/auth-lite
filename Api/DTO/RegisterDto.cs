using System.ComponentModel.DataAnnotations;

namespace Api.DTO;

public class RegisterDto
{
    [EmailAddress]
    public required string Email { get; set; }
    [MinLength(8)]
    public required string Password { get; set; }
    public string? Name { get; set; }
    public string? Picture { get; set; }
    public string? Nonce { get; set; } //Todo: Få krav på
}