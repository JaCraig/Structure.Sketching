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
using Structure.Sketching.Numerics.Interfaces;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Equalizes of an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Equalize : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Equalize"/> class.
        /// </summary>
        /// <param name="histogram">The histogram.</param>
        public Equalize(IHistogram histogram = null)
        {
            Histogram = histogram ?? new RGBHistogram();
        }

        /// <summary>
        /// Gets or sets the histogram.
        /// </summary>
        /// <value>
        /// The histogram.
        /// </value>
        private IHistogram Histogram { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            Histogram.LoadImage(image)
                     .Equalize();
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var TempR = *TargetPointer2;
                        var TempG = *(TargetPointer2 + 1);
                        var TempB = *(TargetPointer2 + 2);
                        var ResultColor = Histogram.EqualizeColor(new Color(TempR, TempG, TempB));
                        (*TargetPointer2) = ResultColor.Red;
                        ++TargetPointer2;
                        (*TargetPointer2) = ResultColor.Green;
                        ++TargetPointer2;
                        (*TargetPointer2) = ResultColor.Blue;
                        TargetPointer2 += 2;
                    }
                }
            });
            return image;
        }
    }
}