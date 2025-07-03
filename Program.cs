using System.Drawing;
using System.Runtime.Versioning;
using ImageColorQuantizer.Services;
using ImageColorQuantizer.Utils;

class Program
{
    [SupportedOSPlatform("windows")]
    static void Main(string[] args)
    {
        if (args.Length == 0 || !File.Exists(args[0]))
        {
            Console.WriteLine("Usage: ImageColorQuantizer <image_path> [color count]");
            return;
        }

        string path = args[0];
        int    colorCount = args.Length > 1 && int.TryParse(args[1], out int count) ? count : 4;

        Bitmap bitmap = new Bitmap(path);
        var pixels = ImageLoader.ExtractPixels(bitmap);
        var rgbTuples = ColorUtils.ConvertToRGBTuples(pixels);

        rgbTuples = rgbTuples.Distinct().ToList(); // Ensure distinct colors

        var palette = QuantizationService.KmeansQuantizer(rgbTuples, colorCount);
        palette = palette
            .OrderBy(c => c.R + c.G + c.B) // Brightness sort
            .ToList();
        var quantizedImage = QuantizationService.RecolorImage(bitmap, palette);

        Directory.CreateDirectory("ImageOutput");
        string outputPath = Path.Combine("ImageOutput", "quantized.png");
        quantizedImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);

        Console.WriteLine($"Saved quantized image to: {outputPath}");
    }
}