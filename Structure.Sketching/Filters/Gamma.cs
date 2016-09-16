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
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Adjusts gamma of an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Gamma : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Gamma"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Gamma(float value)
        {
            Value = value;
            Ramp = new int[256];
            Parallel.For(0, 256, x =>
            {
                Ramp[x] = (int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5);
                Ramp[x] = Ramp[x] < 0 ? 0 : Ramp[x] > 255 ? 255 : Ramp[x];
            });
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public float Value { get; set; }

        private int[] Ramp { get; set; }

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
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        *TargetPointer2 = (byte)Ramp[*TargetPointer2];
                        ++TargetPointer2;
                        *TargetPointer2 = (byte)Ramp[*TargetPointer2];
                        ++TargetPointer2;
                        *TargetPointer2 = (byte)Ramp[*TargetPointer2];
                        TargetPointer2 += 2;
                    }
                }
            });
            return image;
        }
    }
}