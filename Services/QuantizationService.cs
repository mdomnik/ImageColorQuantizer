using System.Drawing;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;

namespace ImageColorQuantizer.Services
{
    internal class QuantizationService
    {
        //quantizes an image by forming a palette of colors and then matching each pixel to the closest relative palette color
        public static List<(int R, int G, int B)> KmeansQuantizer(List<(int R, int G, int B)> pixels, int k, int maxIterations = 10)
        {
            // transforms the input pixels to a list of distinct colors
            var distinctColors = pixels.Distinct().ToList();

            // if the number of distinct colors is less than or equal to k, return them directly
            if (distinctColors.Count <= k)
            {
                return distinctColors;
            }

            var random = new Random();

            // select k initial centroids from the distinct colors
            var centroids = pixels
                            .GroupBy(p => p)
                            .OrderByDescending(g => g.Count()) // most frequent first
                            .Take(k)
                            .Select(g => g.Key)
                            .ToList();

            for (int iter = 0; iter < maxIterations; iter++)
            {

                var clusters = new List<List<(int R, int G, int B)>>();
                // Initialize clusters
                for (int i = 0; i < k; i++) clusters.Add(new List<(int, int, int)>());

                // Assign each pixel to the closest centroid
                foreach (var pixel in pixels)
                {
                    int closestIndex = GetClosestCentroid(pixel, centroids);
                    clusters[closestIndex].Add(pixel);
                }
                //If no pixels were assigned to a centroid, reinitialize it with an average color
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

        // finds the closest centroid to the provided pixel color
        private static int GetClosestCentroid((int R, int G, int B) color, List<(int R, int G, int B)> centroids)
        {
            int ClosestIndex = 0;
            double minDistance = double.MaxValue;

            // for each centroid, calculate the squared distance to the color
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

        // recolors an image based by finding the closest palette color and changing it's color to it
        public static Bitmap RecolorImage(Bitmap original, List<(int R, int G, int B)> palette)
        {
            var newImage = new Bitmap(original.Width, original.Height);
            var cache = new Dictionary<(int, int, int), (int, int, int)>();

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    var pixel = original.GetPixel(x, y);

                    //ingore transparent pixels
                    if (pixel.A < 255)
                    {
                        newImage.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                        continue;
                    }
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

        //compares a given pixel against a palette of RGB coordinates
        private static (int R, int G, int B) FindClosestPaletteColor((int R, int G, int B) pixel, List<(int R, int G, int B)> palette)
        {
            double minDistance = double.MaxValue;
            (int R, int G, int B) closest = palette[0];

            foreach (var c in palette)
            {
                double distance = Math.Pow(pixel.R - c.R, 2) + Math.Pow(pixel.G - c.G, 2) + Math.Pow(pixel.B - c.B, 2);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = c;
                }
            }
            return closest;
        }
    }
}
