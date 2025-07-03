using System.Drawing;
using System.Runtime.Versioning;
using ImageColorQuantizer.Services;
using ImageColorQuantizer.Utils;

class Program
{
    [SupportedOSPlatform("windows")]
    static void Main(string[] args)
    {
        // ensures there is a valid image path
        if (args.Length == 0 || !File.Exists(args[0]))
        {
            Console.WriteLine("Usage: ImageColorQuantizer <image_path> [color count]");
            return;
        }

        string path = args[0];
        int    colorCount = args.Length > 1 && int.TryParse(args[1], out int count) ? count : 4;

        // loads the image and processes it
        Bitmap bitmap = new Bitmap(path);

        //gets the pixels from the image
        var pixels = ImageLoader.ExtractPixels(bitmap);

        // converts the pixels to RGB tuples
        var rgbTuples = ColorUtils.ConvertToRGBTuples(pixels);

        // removes duplicates
        rgbTuples = rgbTuples.Distinct().ToList();

        // quantizes the image using K-means algorithm and order the palette by brightness
        var palette = QuantizationService.KmeansQuantizer(rgbTuples, colorCount);
        palette = palette
            .OrderBy(c => c.R + c.G + c.B) // Brightness sort
            .ToList();

        // converts the image to the quantized palette
        var quantizedImage = QuantizationService.RecolorImage(bitmap, palette);

        // saves the quantized image to the output directory
        Directory.CreateDirectory("ImageOutput");
        string outputPath = Path.Combine("ImageOutput", "quantized.png");
        quantizedImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine($"Saved quantized image to: {outputPath}");
    }
}