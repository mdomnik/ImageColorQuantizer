﻿using System.Drawing;

namespace ImageColorQuantizer.Utils
{
    internal class ColorUtils
    {
        // helper method to convert a list of Color objects to a list of RGB tuples
        public static List<(int R, int G, int B)> ConvertToRGBTuples(List<Color> colors)
        {
            return colors.Select(c => ((int)c.R, (int)c.G, (int)c.B)).ToList();
        }
    }
}
