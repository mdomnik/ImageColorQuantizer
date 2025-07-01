using System.Drawing;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;

namespace ImageColorQuantizer.Services
{
    internal class QuantizationService
    {
        public static List<(int R, int G, int B)> KmeansQuantizer(List<(int R, int G, int B)> pixels, int k, int maxIterations = 10)
        {
            var distinctColors = pixels.Distinct().ToList();

            if (distinctColors.Count <= k)
            {
                return distinctColors;
            }

            var random = new Random();
            var centroids = pixels.OrderBy(x => random.Next()).Take(k).ToList();


            for (int iter = 0; iter < maxIterations; iter++)
            {
                var clusters = new List<List<(int R, int G, int B)>>();
                for (int i = 0; i < k; i++) clusters.Add(new List<(int, int, int)>());

                foreach (var pixel in pixels)
                {
                    int closestIndex = GetClosestCentroid(pixel, centroids);
                    clusters[closestIndex].Add(pixel);
                }

                for (int i = 0; i < k; i++)
                {
                    if (clusters[i].Count == 0) continue;

                    int avrR = (int)clusters[i].Average(p => p.R);
                    int avrG = (int)clusters[i].Average(p => p.G);
                    int avrB = (int)clusters[i].Average(p => p.B);

                    centroids[i] = (avrR, avrG, avrB);
                }
            }
            return centroids;
        }

        private static int GetClosestCentroid((int R, int G, int B) color, List<(int R, int G, int B)> centroids)
        {
            int ClosestIndex = 0;
            double minDistance = double.MaxValue;

            for (int i = 0; i < centroids.Count; i++)
            {
                var c = centroids[i];
                double distance = Math.Pow(color.R - c.R, 2) + Math.Pow(color.G - c.G, 2) + Math.Pow(color.B - c.B, 2);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    ClosestIndex = i;
                }
            }
            return ClosestIndex;
        }

        [SupportedOSPlatform("windows")]
        public static Bitmap RecolorImage(Bitmap original, List<(int R, int G, int B)> palette)
        {
            var newImage = new Bitmap(original.Width, original.Height);
            var cache = new Dictionary<(int, int, int), (int, int, int)>();

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    var pixel = original.GetPixel(x, y);
                    var key = (pixel.R, pixel.G, pixel.B);

                    if (!cache.TryGetValue(key, out var mapped))
                    {
                        mapped = FindClosestPaletteColor(key, palette);
                        cache[key] = mapped;
                    }

                    newImage.SetPixel(x, y, Color.FromArgb(mapped.Item1, mapped.Item2, mapped.Item3));
                }
            }
            return newImage;
        }

        private static (int R, int G, int B) FindClosestPaletteColor((int R, int G, int B) pixel, List<(int R, int G, int B)> palette)
        {
            double mindistance = double.MaxValue;
            (int R, int G, int B) closest = palette[0];

            foreach (var c in palette)
            {
                double distance = Math.Pow(pixel.R - c.R, 2) + Math.Pow(pixel.G - c.G, 2) + Math.Pow(pixel.B - c.B, 2);
                if (distance < mindistance)
                {
                    mindistance = distance;
                    closest = c;
                }
            }
            return closest;
        }
    }
}
