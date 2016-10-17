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

using Structure.Sketching.ExtensionMethods;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Effects
{
    /// <summary>
    /// Pixellates an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Pixellate : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pixellate"/> class.
        /// </summary>
        /// <param name="pixelSize">Size of the pixel.</param>
        public Pixellate(int pixelSize)
        {
            PixelSize = pixelSize;
        }

        /// <summary>
        /// Gets or sets the size of the pixel.
        /// </summary>
        /// <value>The size of the pixel.</value>
        public int PixelSize { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            for (int y = targetLocation.Bottom; y < targetLocation.Top; y += PixelSize)
            {
                var MinY = (y - (PixelSize / 2)).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                var MaxY = (y + (PixelSize / 2)).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; x += PixelSize)
                    {
                        uint RValue = 0;
                        uint GValue = 0;
                        uint BValue = 0;
                        var MinX = (x - (PixelSize / 2)).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        var MaxX = (x + (PixelSize / 2)).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        int NumberPixels = 0;
                        for (int x2 = MinX; x2 < MaxX; ++x2)
                        {
                            for (int y2 = MinY; y2 < MaxY; ++y2)
                            {
                                RValue += image.Pixels[((y * image.Width) + x) * 4];
                                GValue += image.Pixels[(((y * image.Width) + x) * 4) + 1];
                                BValue += image.Pixels[(((y * image.Width) + x) * 4) + 2];
                                ++NumberPixels;
                            }
                        }
                        RValue /= (uint)NumberPixels;
                        GValue /= (uint)NumberPixels;
                        BValue /= (uint)NumberPixels;
                        Parallel.For(MinX, MaxX, x2 =>
                        {
                            for (int y2 = MinY; y2 < MaxY; ++y2)
                            {
                                image.Pixels[((y2 * image.Width) + x2) * 4] = (byte)RValue;
                                image.Pixels[(((y2 * image.Width) + x2) * 4) + 1] = (byte)GValue;
                                image.Pixels[(((y2 * image.Width) + x2) * 4) + 2] = (byte)BValue;
                            }
                        });
                    }
                }
            }
            return image;
        }
    }
}