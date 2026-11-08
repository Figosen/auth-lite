using Client.Util;

public record EncodedJWTTokens(string AccessToken, string IdToken, string RefreshToken);
public record DecodedJWTTokens(AccessTokenPayload AccessToken, IdTokenPayload IdToken, string RefreshToken);
public record JwtHeader(string alg, string typ);