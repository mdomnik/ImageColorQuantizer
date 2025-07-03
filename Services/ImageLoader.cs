using System.Drawing;
using System.Runtime.Versioning;

namespace ImageColorQuantizer.Services
{
    [SupportedOSPlatform("windows")]
    internal class ImageLoader
    {
        // Extracts non-transparent pixels from a bitmap image
        public static List<Color> ExtractPixels(Bitmap bitmap)
        {
            var pixels = new List<Color>();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);

                    if (color.A > 0)
                        pixels.Add(color);
                }
            }
            return pixels;
        }
    }
}
