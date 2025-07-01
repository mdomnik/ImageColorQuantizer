using System.Drawing;
using System.Runtime.Versioning;

namespace ImageColorQuantizer.Services
{
    internal class ImageLoader
    {
        [SupportedOSPlatform("windows")]
        public static List<Color> ExtractPixels(Bitmap bitmap)
        {
            var pixels = new List<Color>();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    pixels.Add(bitmap.GetPixel(x, y));
                }
            }
            return pixels;
        }
    }
}
