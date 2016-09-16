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

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Solarizes an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Solarize : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Solarize"/> class.
        /// </summary>
        /// <param name="threshold">The threshold (between 0 and 3).</param>
        public Solarize(float threshold)
        {
            Threshold = threshold;
        }

        /// <summary>
        /// Gets or sets the threshold.
        /// </summary>
        /// <value>The threshold.</value>
        public float Threshold { get; set; }

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
                fixed (byte* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    byte* OutputPointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        if (Distance.Euclidean(new Color(*OutputPointer, *(OutputPointer + 1), *(OutputPointer + 2)), Color.Black) < Threshold)
                        {
                            (*OutputPointer) = (byte)(255 - (*OutputPointer));
                            ++OutputPointer;
                            (*OutputPointer) = (byte)(255 - (*OutputPointer));
                            ++OutputPointer;
                            (*OutputPointer) = (byte)(255 - (*OutputPointer));
                            OutputPointer += 2;
                        }
                        OutputPointer += 4;
                    }
                }
            });
            return image;
        }
    }
}