using Structure.Sketching.Colors;
using System;

namespace Structure.Sketching.Numerics
{
    /// <summary>
    /// Contains distance related utilities
    /// </summary>
    public static class Distance
    {
        /// <summary>
        /// Gets the Euclidean distance between two colors
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The distance between the colors</returns>
        public static double Euclidean(Color color1, Color color2)
        {
            int Red = color1.Red - color2.Red;
            int Green = color1.Green - color2.Green;
            int Blue = color1.Blue - color2.Blue;
            int Alpha = color1.Alpha - color2.Alpha;
            Red = Red * Red;
            Green = Green * Green;
            Blue = Blue * Blue;
            Alpha = Alpha * Alpha;

            return Math.Sqrt(Red + Green + Blue + Alpha);
        }
    }
}