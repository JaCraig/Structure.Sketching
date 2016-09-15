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
    /// Equalizes of an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Equalize : IFilter
    {
        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var TempHistogram = new RGBHistogram(image);
            TempHistogram.Equalize();
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* TargetPointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        (*TargetPointer2).X = TempHistogram.R[(int)((*TargetPointer2).X * 255f)] / 255f;
                        (*TargetPointer2).Y = TempHistogram.G[(int)((*TargetPointer2).Y * 255f)] / 255f;
                        (*TargetPointer2).Z = TempHistogram.B[(int)((*TargetPointer2).Z * 255f)] / 255f;
                        ++TargetPointer2;
                    }
                }
            });
            return image;
        }
    }
}