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
            H = new double[361];
            V = new double[101];
            S = new double[101];
            if (image != null)
                LoadImage(image);
        }

        /// <summary>
        /// Blue values
        /// </summary>
        public double[] H { get; set; }

        /// <summary>
        /// Green values
        /// </summary>
        public double[] S { get; set; }

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
            int HMax = int.MinValue;
            int HMin = int.MaxValue;
            int SMax = int.MinValue;
            int SMin = int.MaxValue;
            int VMax = int.MinValue;
            int VMin = int.MaxValue;
            for (int x = 0; x < 361; ++x)
            {
                if (H[x] > 0f)
                {
                    if (HMax < x)
                        HMax = x;
                    if (HMin > x)
                        HMin = x;
                }
            }
            for (int x = 0; x < 101; ++x)
            {
                if (S[x] > 0f)
                {
                    if (SMax < x)
                        SMax = x;
                    if (SMin > x)
                        SMin = x;
                }
                if (V[x] > 0f)
                {
                    if (VMax < x)
                        VMax = x;
                    if (VMin > x)
                        VMin = x;
                }
            }

            double PreviousH = H[0];
            H[0] = H[0] * 360 / TotalPixels;
            double PreviousS = S[0];
            S[0] = S[0] * 100 / TotalPixels;
            double PreviousV = V[0];
            V[0] = V[0] * 100 / TotalPixels;
            for (int x = 1; x < 361; ++x)
            {
                PreviousH += H[x];
                H[x] = ((PreviousH - H[HMin]) / (TotalPixels - H[HMin])) * 360;
            }
            for (int x = 1; x < 101; ++x)
            {
                PreviousS += S[x];
                PreviousV += V[x];
                S[x] = ((PreviousS - S[SMin]) / (TotalPixels - S[SMin]));
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
            return new HSV(H[(int)TempHSV.Hue], S[(int)(TempHSV.Saturation * 100)], V[(int)TempHSV.Value * 100]);
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
            Array.Clear(H, 0, H.Length);
            Array.Clear(S, 0, S.Length);
            Array.Clear(V, 0, V.Length);
            fixed (byte* TargetPointer = &image.Pixels[0])
            {
                byte* TargetPointer2 = TargetPointer;
                for (int x = 0; x < image.Width; ++x)
                {
                    for (int y = 0; y < image.Height; ++y)
                    {
                        var TempR = *TargetPointer2;
                        ++TargetPointer2;
                        var TempG = *TargetPointer2;
                        ++TargetPointer2;
                        var TempB = *TargetPointer2;
                        TargetPointer2 += 2;
                        var TempHSV = (HSV)new Color(TempR, TempG, TempB);
                        ++H[(int)TempHSV.Hue];
                        ++S[(int)(TempHSV.Saturation * 100)];
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
            for (int x = 0; x < 361; ++x)
            {
                H[x] /= TotalPixels;
            }
            for (int x = 0; x < 101; ++x)
            {
                S[x] /= TotalPixels;
                V[x] /= TotalPixels;
            }
            return this;
        }
    }
}