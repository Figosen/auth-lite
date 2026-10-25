using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace Client.Util;

public class ProfileCard
{
    public async Task RenderAsync(IdTokenPayload payload)
    {

        string roundedPath = "rounded_avatar.png";
        string inputPath = "recieved.png";
        await File.WriteAllBytesAsync(inputPath, Convert.FromBase64String(payload.Picture));


        // 1. Gør billedet kvadratisk og cirkulært
        using (var image = Image.Load<Rgba32>(inputPath))
        {
            int minDim = Math.Min(image.Width, image.Height);

            image.Mutate(x => x.Crop(new Rectangle(
                (image.Width - minDim) / 2,
                (image.Height - minDim) / 2,
                minDim,
                minDim
            )));

            int radius = minDim / 2;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int dx = x - radius;
                    int dy = y - radius;
                    if (dx * dx + dy * dy > radius * radius)
                    {
                        image[x, y] = new Rgba32(0, 0, 0, 0);
                    }
                }
            }

            image.Save(roundedPath);
        }

        // 2. Kør chafa og fang outputtet som tekst (i stedet for at printe direkte)
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "chafa",
            Arguments = "--size=16x8 rounded_avatar.png",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        string chafaOutput = "";
        using (var process = System.Diagnostics.Process.Start(psi))
        {
            if (process != null)
            {
                chafaOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
        }

        string[] imageLines = chafaOutput.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        string[] textLines =
        [
            $"  Navn:    {payload.Name ?? "Unknown"}",
            $"  Email:   {payload.Email ?? "Unknown"}",
            $"  Id:      {payload.Sub}"
        ];

        Console.Clear();
        Console.WriteLine("==================================================");

        // Beregn startlinjen for teksten, så den lander midt i billedets højde
        int paddingTop = Math.Max(0, (imageLines.Length - textLines.Length) / 2);

        for (int i = 0; i < imageLines.Length; i++)
        {
            string imgLine = imageLines[i];
            string txtLine = "";

            int textIndex = i - paddingTop;
            if (textIndex >= 0 && textIndex < textLines.Length)
            {
                txtLine = textLines[textIndex];
            }

            Console.WriteLine($"{imgLine}{txtLine}");
        }

        Console.WriteLine("==================================================");

        if (File.Exists(roundedPath))
        {
            File.Delete(roundedPath);
        }
    }
}