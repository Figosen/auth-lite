using Client.Util;

class Program
{
    static async Task Main()
    {
        var client = new HttpClient();
        var url = "http://localhost:5042/Auth";
        var presentationLogic = new PresentationLogic(client,url);
        int selection;
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
            case 1: // TODO: Lav en rigtig switch pattern
                await presentationLogic.Login();
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}