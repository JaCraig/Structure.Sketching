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
    /// Adds randomization to each pixel in an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Jitter : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Jitter"/> class.
        /// </summary>
        /// <param name="amount">The amount of potential randomization.</param>
        public Jitter(int amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount { get; set; }

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
                    Color* SourcePointer = Pointer;
                    for (int x = 0; x < image.Width; ++x)
                    {
                        var NewX = Random.ThreadSafeNext(-Amount, Amount);
                        var NewY = Random.ThreadSafeNext(-Amount, Amount);
                        NewX += x;
                        NewY += y;
                        NewX = NewX < targetLocation.Left ? targetLocation.Left : NewX >= targetLocation.Right ? targetLocation.Right - 1 : NewX;
                        NewY = NewY < targetLocation.Bottom ? targetLocation.Bottom : NewY >= targetLocation.Top ? targetLocation.Top - 1 : NewY;
                        image.Pixels[(NewY * image.Width) + NewX] = *SourcePointer;
                        ++SourcePointer;
                    }
                }
            });
            return image;
        }
    }
}