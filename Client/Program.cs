using System;
using System.IO;
using System.Net.Http.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Api.DTO;
using Client.Util;

class Program
{
    static async Task Main()
    {
        var client = new HttpClient();
        var url = "http://localhost:5042/Auth";
        var presentationLogic = new PresentationLogic(client,url);
        int selection = 0;
        do
        {
            Console.WriteLine("========== Auth Lite ==========");
            Console.WriteLine("    [1] Login");
            Console.WriteLine("    [2] Registrer");
            Console.WriteLine("    [3] Refresh");
            Console.WriteLine("===============================");
            Console.Write("Skriv et tal: ");
            selection = Convert.ToInt32(Console.ReadLine());
        } while (selection < 3 && selection > 1);

        switch (selection)
        {
            case 1:
                await presentationLogic.Login();
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}

public record EncodedJWTTokens(string AccessToken, string IdToken, string RefreshToken);
public record DecodedJWTTokens(AccessTokenPayload AccessToken, IdTokenPayload IdToken, string RefreshToken);