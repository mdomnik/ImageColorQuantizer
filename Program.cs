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

        Console.WriteLine($"loaded {pixels.Count} pixels from image.");
        Console.WriteLine($"Image Dimensions: {bitmap.Width}x{bitmap.Height}");
        Console.WriteLine($"Sample: {rgbTuples[0]}");
    }
}