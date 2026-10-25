using System.Text.Json.Serialization;

namespace Client.Util;
public record AccessTokenPayload(
    [property: JsonPropertyName("sub")] string Sub,
    [property: JsonPropertyName("exp")] long Exp,
    [property: JsonPropertyName("iat")] long Iat,
    [property: JsonPropertyName("iss")] string Iss,
    [property: JsonPropertyName("aud")] string Aud);

public record IdTokenPayload(
    [property: JsonPropertyName("sub")] string Sub,
    [property: JsonPropertyName("exp")] long Exp,
    [property: JsonPropertyName("iat")] long Iat,
    [property: JsonPropertyName("iss")] string Iss,
    [property: JsonPropertyName("aud")] string Aud,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("picture")] string? Picture,
    [property: JsonPropertyName("nonce")] string? Nonce);