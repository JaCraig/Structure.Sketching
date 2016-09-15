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

using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
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
        public Noise(double amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public double Amount { get; set; }

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
                fixed (Vector4* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* OutputPointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        float R = (*OutputPointer).X + (float)Numerics.Random.ThreadSafeNextDouble(-Amount, Amount);
                        float G = (*OutputPointer).Y + (float)Numerics.Random.ThreadSafeNextDouble(-Amount, Amount);
                        float B = (*OutputPointer).Z + (float)Numerics.Random.ThreadSafeNextDouble(-Amount, Amount);
                        R = R < 0 ? 0 : R > 1 ? 1 : R;
                        G = G < 0 ? 0 : G > 1 ? 1 : G;
                        B = B < 0 ? 0 : B > 1 ? 1 : B;
                        (*OutputPointer).X = R;
                        (*OutputPointer).Y = G;
                        (*OutputPointer).Z = B;
                        OutputPointer++;
                    }
                }
            });
            return image;
        }
    }
}