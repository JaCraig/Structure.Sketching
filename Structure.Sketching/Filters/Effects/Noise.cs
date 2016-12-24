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
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Effects
{
    /// <summary>
    /// Adds randomization to an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Noise : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Noise"/> class.
        /// </summary>
        /// <param name="amount">The amount of potential randomization (0 to 1).</param>
        public Noise(byte amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public byte Amount { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Color* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Color* OutputPointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        int R = (*OutputPointer) + Random.ThreadSafeNext(-Amount, Amount);
                        int G = *(OutputPointer + 1) + Random.ThreadSafeNext(-Amount, Amount);
                        int B = *(OutputPointer + 2) + Random.ThreadSafeNext(-Amount, Amount);
                        R = R < 0 ? 0 : R > 255 ? 255 : R;
                        G = G < 0 ? 0 : G > 255 ? 255 : G;
                        B = B < 0 ? 0 : B > 255 ? 255 : B;
                        (*OutputPointer).Red = (byte)R;
                        (*OutputPointer).Green = (byte)G;
                        (*OutputPointer).Blue = (byte)B;
                        ++OutputPointer;
                    }
                }
            });
            return image;
        }
    }
}