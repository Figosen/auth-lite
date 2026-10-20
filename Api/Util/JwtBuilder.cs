using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Api.Util;
// TODO kan måske gøres mere SOLID ved CI
public record JwtHeader(string alg, string typ);
public class JwtBuilder
{
    private readonly string _secretKey;

    public JwtBuilder(string secretKey)
    {
        _secretKey = secretKey;
    }

    public string CreateToken<T>(JwtHeader header, T payload)
    {
        var jsonHeader = JsonSerializer.Serialize(header);
        var jsonPayload = JsonSerializer.Serialize(payload);

        var encodedHeader = Base64UrlEncoder.Encode(jsonHeader);
        var encodedPayload = Base64UrlEncoder.Encode(jsonPayload);
        var message = $"{encodedHeader}.{encodedPayload}";
        
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey))) // TODO: Kig mere ind i hvordan cryptografien egentlig fungerer
        {
            string signature = Base64UrlEncoder.Encode(hmac.ComputeHash(Encoding.UTF8.GetBytes(message)));
            return $"{message}.{signature}";
        }
    }
}
