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
using Structure.Sketching.ExtensionMethods;
using Structure.Sketching.Quantizers.BaseClasses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structure.Sketching.Quantizers.Octree
{
    /// <summary>
    /// Octree quantizer class
    /// </summary>
    /// <seealso cref="QuantizerBase" />
    public class OctreeQuantizer : QuantizerBase
    {
        /// <summary>
        /// Maximum allowed color depth
        /// </summary>
        private int colors;

        /// <summary>
        /// Stores the tree
        /// </summary>
        private Octree octree;

        /// <summary>
        /// Gets the palette.
        /// </summary>
        /// <returns>
        /// The list of colors in the palette
        /// </returns>
        protected override Bgra[] GetPalette()
        {
            List<Bgra> palette = octree.Palletize(Math.Max(colors, 1));
            palette.Add(new Color(0, 0, 0, 0));
            TransparentIndex = colors;
            return palette.ToArray();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="maxColors">Maximum colors</param>
        protected override void Initialize(Image image, int maxColors)
        {
            colors = maxColors.Clamp(1, 255);

            if (octree == null)
            {
                octree = new Octree(GetBitsNeededForColorDepth(maxColors));
            }
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var Pixel = image.Pixels[(y * image.Width) + x];
                    octree.AddColor(new Color(Pixel));
                }
            }
        }

        /// <summary>
        /// Processes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        /// The resulting byte array.
        /// </returns>
        protected override byte[] Process(Image image)
        {
            byte[] quantizedPixels = new byte[image.Width * image.Height];
            Parallel.For(
                0,
                image.Height,
                y =>
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Bgra sourcePixel = new Color(image.Pixels[(y * image.Width) + x]);
                        quantizedPixels[(y * image.Width) + x] = QuantizePixel(sourcePixel);
                    }
                });

            return quantizedPixels;
        }

        /// <summary>
        /// Quantizes the pixel.
        /// </summary>
        /// <param name="pixel">The pixel.</param>
        /// <returns>The resulting byte</returns>
        protected byte QuantizePixel(Bgra pixel)
        {
            var paletteIndex = (byte)colors;
            if (pixel.Alpha > TransparencyThreshold)
            {
                paletteIndex = (byte)octree.GetPaletteIndex(pixel);
            }

            return paletteIndex;
        }

        /// <summary>
        /// Gets the bits needed for color depth.
        /// </summary>
        /// <param name="colorCount">The color count.</param>
        /// <returns>The bits needed</returns>
        private int GetBitsNeededForColorDepth(int colorCount)
        {
            return (int)Math.Ceiling(Math.Log(colorCount, 2));
        }
    }
}