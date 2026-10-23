using System.Security.Cryptography;
using Api.BLL.Context;
using Api.DTO;
using Api.Util;
using Microsoft.IdentityModel.Tokens;

public interface IAuthLogic
{
    JWTTokens LoginLogic(LoginDto login);
    JWTTokens RefreshLogic(string userId);
}

public record JWTTokens(string AccessToken, string IdToken, string RefreshToken);

public class AuthLogic(JwtBuilder jwtBuilder, AuthDbContext dbContext) : IAuthLogic
{
    private JwtHeader jwtHeader = new(Constants.Auth.HS256, Constants.Auth.JWT);
    public JWTTokens LoginLogic(LoginDto login)
    {
        return new JWTTokens
        (
            GenerateAccessToken(""),
            GenerateIdToken(""),
            GenerateRefreshToken(" ")
        );
    }
    public JWTTokens RefreshLogic(string userId)
    {
        return null;
    }
    public JWTTokens RegisterLogic(string userId)
    {
        return null;
    }
    string GenerateAccessToken(string userId)
    {
        return jwtBuilder.CreateToken(jwtHeader, new AccessTokenPayload("",0,0,"",""));
    }
    string GenerateIdToken(string userId)
    {
        return jwtBuilder.CreateToken(jwtHeader, new IdTokenPayload("",0,0,"","","","","",""));
    }
    string GenerateRefreshToken(string userId)
    {
        return Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32));
    }
}