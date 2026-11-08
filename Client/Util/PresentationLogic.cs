using System.Net.Http.Json;
using System.Text.Json;
using Api.DTO;
using Microsoft.IdentityModel.Tokens;

namespace Client.Util;
public record JWTTokens(string AccessToken, string IdToken, string RefreshToken);
public class PresentationLogic(HttpClient client, string url)
{
    public async Task Login()
    {
        Console.Clear();
        Console.WriteLine("========== Auth Lite ==========");
        Console.Write("    Email: ");
        var email = Console.ReadLine();
        Console.Write("    Password: ");
        var password = Console.ReadLine();
        Console.WriteLine("===============================");
        var dto = new LoginDto { Email = email, Password = password };
        var response = await client.PostAsJsonAsync($"{url}/login", dto);
        response.EnsureSuccessStatusCode();

        var tokens = await response.Content.ReadFromJsonAsync<JWTTokens>();

        var payloadPart = tokens!.IdToken.Split('.')[1];
        var json = Base64UrlEncoder.Decode(payloadPart);
        var idToken = JsonSerializer.Deserialize<IdTokenPayload>(json)!;

        var profileCardRenderer = new ProfileCard();
        await profileCardRenderer.RenderAsync(idToken);
        
    }
}