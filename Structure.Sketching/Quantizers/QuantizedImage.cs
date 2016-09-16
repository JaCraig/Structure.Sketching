/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Structure.Sketching.Colors;
using Structure.Sketching.Colors.ColorSpaces;
using System;
using System.Threading.Tasks;

namespace Structure.Sketching.Quantizers
{
    /// <summary>
    /// A quantized image
    /// </summary>
    public class QuantizedImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantizedImage"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="pixels">The pixels.</param>
        /// <param name="transparentIndex">Index of the transparent.</param>
        public QuantizedImage(int width, int height, Bgra[] palette, byte[] pixels, int transparentIndex = -1)
        {
            if (width <= 0) width = 1;
            if (height <= 0) height = 1;
            Width = width;
            Height = height;
            Palette = palette;
            Pixels = pixels;
            TransparentIndex = transparentIndex;
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the palette.
        /// </summary>
        /// <value>The palette.</value>
        public Bgra[] Palette { get; private set; }

        /// <summary>
        /// Gets the pixels.
        /// </summary>
        /// <value>The pixels.</value>
        public byte[] Pixels { get; private set; }

        /// <summary>
        /// Gets the index of the transparent color.
        /// </summary>
        /// <value>The index of the transparent color.</value>
        public int TransparentIndex { get; private set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="QuantizedImage"/> to <see cref="Image"/>.
        /// </summary>
        /// <param name="quantizedImage">The quantized image.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Image(QuantizedImage quantizedImage)
        {
            int palletCount = quantizedImage.Palette.Length - 1;
            byte[] Pixels = new byte[quantizedImage.Pixels.Length * 4];

            Parallel.For(0, quantizedImage.Pixels.Length, x =>
            {
                int offset = x * 4;
                Color color = quantizedImage.Palette[Math.Min(palletCount, quantizedImage.Pixels[x])];
                Pixels[offset] = color.Red;
                Pixels[offset + 1] = color.Green;
                Pixels[offset + 2] = color.Blue;
                Pixels[offset + 3] = color.Alpha;
            });

            return new Image(quantizedImage.Width, quantizedImage.Height, Pixels);
        }
    }
}