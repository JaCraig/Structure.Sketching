using Structure.Sketching.Colors;
using Structure.Sketching.Colors.ColorSpaces;
using Structure.Sketching.Numerics.Interfaces;
using System;

namespace Structure.Sketching.Numerics
{
    /// <summary>
    /// Class used to create an RGB Histogram
    /// </summary>
    public class HSVHistogram : IHistogram
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HSVHistogram"/> class.
        /// </summary>
        /// <param name="image">The image to load.</param>
        public HSVHistogram(Image image = null)
        {
            V = new double[101];
            if (image != null)
                LoadImage(image);
        }

        /// <summary>
        /// Red values
        /// </summary>
        public double[] V { get; set; }

        private int Height;

        private int Width;

        /// <summary>
        /// Equalizes the histogram
        /// </summary>
        /// <returns>this</returns>
        public IHistogram Equalize()
        {
            double TotalPixels = Width * Height;
            int VMax = int.MinValue;
            int VMin = int.MaxValue;
            for (int x = 0; x < 101; ++x)
            {
                if (V[x] > 0f)
                {
                    if (VMax < x)
                        VMax = x;
                    if (VMin > x)
                        VMin = x;
                }
            }
            double PreviousV = V[0];
            V[0] = (V[0] / TotalPixels);
            for (int x = 1; x < 101; ++x)
            {
                PreviousV += V[x];
                V[x] = ((PreviousV - V[VMin]) / (TotalPixels - V[VMin]));
            }
            Width = 256;
            Height = 1;
            return this;
        }

        /// <summary>
        /// Equalizes the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The resulting color</returns>
        public Color EqualizeColor(Color color)
        {
            var TempHSV = (HSV)color;
            return new HSV(TempHSV.Hue, TempHSV.Saturation, V[(int)Math.Round(TempHSV.Value * 100, MidpointRounding.AwayFromZero)]);
        }

        /// <summary>
        /// Loads the specified colors.
        /// </summary>
        /// <param name="colors">The colors.</param>
        /// <returns>This</returns>
        public IHistogram Load(params Color[] colors)
        {
            Width = colors.Length;
            Height = 1;
            Array.Clear(V, 0, V.Length);
            for (int x = 0; x < colors.Length; ++x)
            {
                var TempHSV = (HSV)colors[x];
                ++V[(int)(TempHSV.Value * 100)];
            }
            return this;
        }

        /// <summary>
        /// Loads an image
        /// </summary>
        /// <param name="image">Image to load</param>
        /// <returns>this</returns>
        public unsafe IHistogram LoadImage(Image image)
        {
            Width = image.Width;
            Height = image.Height;
            Array.Clear(V, 0, V.Length);
            fixed (Color* TargetPointer = &image.Pixels[0])
            {
                Color* TargetPointer2 = TargetPointer;
                for (int x = 0; x < image.Width; ++x)
                {
                    for (int y = 0; y < image.Height; ++y)
                    {
                        var TempR = (*TargetPointer2).Red;
                        var TempG = (*TargetPointer2).Green;
                        var TempB = (*TargetPointer2).Blue;
                        ++TargetPointer2;
                        var TempHSV = (HSV)new Color(TempR, TempG, TempB);
                        ++V[(int)(TempHSV.Value * 100)];
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Normalizes the histogram
        /// </summary>
        /// <returns>this</returns>
        public IHistogram Normalize()
        {
            double TotalPixels = Width * Height;
            if (TotalPixels <= 0)
                return this;
            for (int x = 0; x < 101; ++x)
            {
                V[x] /= TotalPixels;
            }
            return this;
        }
    }
}