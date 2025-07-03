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
            Console.WriteLine("Usage: ImageColorQuantizer <image_path>");
            return;
        }

        string path = args[0];

        Bitmap bitmap = new Bitmap(path);
        var pixels = ImageLoader.ExtractPixels(bitmap);
        var rgbTuples = ColorUtils.ConvertToRGBTuples(pixels);

        rgbTuples = rgbTuples.Distinct().ToList(); // Ensure distinct colors

        int k = 1;
        var palette = QuantizationService.KmeansQuantizer(rgbTuples, k);

        palette = palette
            .OrderBy(c => c.R + c.G + c.B) // Brightness sort
            .ToList();
        var quantizedImage = QuantizationService.RecolorImage(bitmap, palette);

        Console.WriteLine($"loaded {pixels.Count} pixels from image.");
        Console.WriteLine($"Image Dimensions: {bitmap.Width}x{bitmap.Height}");
        Console.WriteLine($"Sample: {rgbTuples[0]}");

        Console.WriteLine($"Generated {palette.Count} palette colors:");
        foreach (var color in palette)
        {
            Console.WriteLine($" - RGB({color.R}, {color.G}, {color.B})");
        }
        Directory.CreateDirectory("ImageOutput");
        string outputPath = Path.Combine("ImageOutput", "quantized.png");
        quantizedImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);

        Console.WriteLine($"Saved quantized image to: {outputPath}");
    }
}