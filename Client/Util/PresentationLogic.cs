using System.Net.Http.Json;
using Api.DTO;

namespace Client.Util;

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
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}