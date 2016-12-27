using Structure.Sketching.Colors;
using Structure.Sketching.Numerics.Interfaces;
using System;

namespace Structure.Sketching.Numerics
{
    /// <summary>
    /// Class used to create an RGB Histogram
    /// </summary>
    public class RGBHistogram : IHistogram
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBHistogram"/> class.
        /// </summary>
        /// <param name="image">The image to load.</param>
        public RGBHistogram(Image image = null)
        {
            R = new float[256];
            G = new float[256];
            B = new float[256];
            if (image != null)
                LoadImage(image);
        }

        /// <summary>
        /// Blue values
        /// </summary>
        public float[] B { get; set; }

        /// <summary>
        /// Green values
        /// </summary>
        public float[] G { get; set; }

        /// <summary>
        /// Red values
        /// </summary>
        public float[] R { get; set; }

        private int Height;

        private int Width;

        /// <summary>
        /// Equalizes the histogram
        /// </summary>
        /// <returns>this</returns>
        public IHistogram Equalize()
        {
            float TotalPixels = Width * Height;
            int RMax = int.MinValue;
            int RMin = int.MaxValue;
            int GMax = int.MinValue;
            int GMin = int.MaxValue;
            int BMax = int.MinValue;
            int BMin = int.MaxValue;
            for (int x = 0; x < 256; ++x)
            {
                if (R[x] > 0f)
                {
                    if (RMax < x)
                        RMax = x;
                    if (RMin > x)
                        RMin = x;
                }
                if (G[x] > 0f)
                {
                    if (GMax < x)
                        GMax = x;
                    if (GMin > x)
                        GMin = x;
                }
                if (B[x] > 0f)
                {
                    if (BMax < x)
                        BMax = x;
                    if (BMin > x)
                        BMin = x;
                }
            }

            float PreviousR = R[0];
            R[0] = R[0] * 256 / TotalPixels;
            float PreviousG = G[0];
            G[0] = G[0] * 256 / TotalPixels;
            float PreviousB = B[0];
            B[0] = B[0] * 256 / TotalPixels;
            for (int x = 1; x < 256; ++x)
            {
                PreviousR += R[x];
                PreviousG += G[x];
                PreviousB += B[x];
                R[x] = ((PreviousR - R[RMin]) / (TotalPixels - R[RMin])) * 255;
                G[x] = ((PreviousG - G[GMin]) / (TotalPixels - G[GMin])) * 255;
                B[x] = ((PreviousB - B[BMin]) / (TotalPixels - B[BMin])) * 255;
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
            return new Color((byte)R[color.Red], (byte)G[color.Green], (byte)B[color.Blue]);
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
            Array.Clear(R, 0, R.Length);
            Array.Clear(G, 0, G.Length);
            Array.Clear(B, 0, B.Length);
            for (int x = 0; x < colors.Length; ++x)
            {
                ++R[colors[x].Red];
                ++G[colors[x].Green];
                ++B[colors[x].Blue];
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
            Array.Clear(R, 0, R.Length);
            Array.Clear(G, 0, G.Length);
            Array.Clear(B, 0, B.Length);
            fixed (Color* TargetPointer = &image.Pixels[0])
            {
                Color* TargetPointer2 = TargetPointer;
                for (int x = 0; x < image.Width; ++x)
                {
                    for (int y = 0; y < image.Height; ++y)
                    {
                        ++R[(*TargetPointer2).Red];
                        ++G[(*TargetPointer2).Green];
                        ++B[(*TargetPointer2).Blue];
                        ++TargetPointer2;
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
            float TotalPixels = Width * Height;
            if (TotalPixels <= 0)
                return this;
            for (int x = 0; x < 256; ++x)
            {
                R[x] /= TotalPixels;
                G[x] /= TotalPixels;
                B[x] /= TotalPixels;
            }
            return this;
        }
    }
}