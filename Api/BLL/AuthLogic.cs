using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using Api.BLL.Context;
using Api.DTO;
using Api.Models.EF;
using Api.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

public interface IAuthLogic
{
    Task<JWTTokens> LoginAsync(LoginDto login);
    Task<JWTTokens> RefreshAsync(RefreshDto refreshDto);
    Task<JWTTokens> RegisterAsync(RegisterDto register);
}

public record JWTTokens(string AccessToken, string IdToken, string RefreshToken);

public class AuthLogic(JwtBuilder jwtBuilder, AuthDbContext dbContext, IConfiguration config) : IAuthLogic
{
    private readonly string _issuer = config["Jwt:Issuer"]!;
    private readonly string _audience = config["Jwt:Audience"]!; // TODO: Magic strings
    private readonly JwtHeader _jwtHeader = new(Constants.Auth.HS256, Constants.Auth.JWT);
    private static readonly TimeSpan RefreshLifetime = TimeSpan.FromDays(7); // TODO: 7 skal være en constant

    public async Task<JWTTokens> LoginAsync(LoginDto login)
    {
        var email = login.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Auths.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null || !BCrypt.Net.BCrypt.EnhancedVerify(login.Password, user.PasswordHash)) throw new AuthenticationException("Invalid email or password");
        
        var (raw, hash, expiresAt) = GenerateRefreshToken();
        user.RefreshTokenHash = hash;
        user.RefreshTokenExpiresAt = expiresAt;
        await dbContext.SaveChangesAsync();

        return new JWTTokens(GenerateAccessToken(user), GenerateIdToken(user), raw);
    }
    
    public async Task<JWTTokens> RefreshAsync(RefreshDto dto)
    {
        var hashedRefresh = HashToken(dto.RefreshToken);
        var user = await dbContext.Auths.FirstOrDefaultAsync(u => u.RefreshTokenHash == hashedRefresh);

        if (user is null || user.RefreshTokenExpiresAt < DateTime.UtcNow)
        throw new AuthenticationException("Invalid refresh token");
        
        var (raw, hash, expiresAt) = GenerateRefreshToken();

        user.RefreshTokenHash = hash;
        user.RefreshTokenExpiresAt = expiresAt;
        await dbContext.SaveChangesAsync();

        return new JWTTokens(GenerateAccessToken(user), GenerateIdToken(user), raw);
    }

    public async Task<JWTTokens> RegisterAsync(RegisterDto register)
    {
        var email = register.Email.Trim().ToLowerInvariant();
        var userExist = await dbContext.Auths.AnyAsync(u => u.Email == email);
        if (userExist) throw new AuthenticationException("Auth error occurred");
        
        var (raw, hash, expiresAt) = GenerateRefreshToken();
        
        var user = new Auth()
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(register.Password),
            Name = register.Name,
            Picture = register.Picture,
            RefreshTokenHash = hash,
            RefreshTokenExpiresAt = expiresAt
        };

        dbContext.Auths.Add(user);
        await dbContext.SaveChangesAsync();
        
        return new JWTTokens(GenerateAccessToken(user), GenerateIdToken(user), raw);
    }
    private string GenerateAccessToken(Auth user)
    {
        var now = DateTimeOffset.UtcNow;
        var payload = new AccessTokenPayload(
            Sub: user.Id.ToString(),
            Exp: now.AddMinutes(15).ToUnixTimeSeconds(), // TODO: 15 skal være en constant
            Iat: now.ToUnixTimeSeconds(),
            Iss: _issuer,
            Aud: _audience);
        return jwtBuilder.CreateToken(_jwtHeader, payload);
    }
    private string GenerateIdToken(Auth user)
    {
        var now = DateTimeOffset.UtcNow;
        var payload = new IdTokenPayload(
        Sub: user.Id.ToString(),
        Exp: now.AddMinutes(15).ToUnixTimeSeconds(),
        Iat: now.ToUnixTimeSeconds(),
        Iss: _issuer,
        Aud: "klient", // TODO: klienter/apps skal kunne registrere sig - kommer senere
        Name: user.Name,
        Email: user.Email,
        Picture: user.Picture,
        Nonce: null);    // TODO: kommer i fase 5

        return jwtBuilder.CreateToken(_jwtHeader, payload);
    }
    private (string raw, string hash, DateTime expiresAt) GenerateRefreshToken() {
        var refresh = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32));
        var hash = HashToken(refresh);
        var refreshTokenExpiresAt = DateTime.UtcNow.Add(RefreshLifetime);
        return (refresh,hash,refreshTokenExpiresAt);
    }
    private string HashToken(string token) => Base64UrlEncoder.Encode(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}