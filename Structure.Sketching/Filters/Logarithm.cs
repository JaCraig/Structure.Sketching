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
using System;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Does a Logarithm function to the image.
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Logarithm : IFilter
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
            var MaxValue = GetMaxValue(image, targetLocation);
            MaxValue = new Color((byte)((255 / Math.Log(1f + MaxValue.Red))),
                (byte)((255 / Math.Log(1f + MaxValue.Green))),
                (byte)((255 / Math.Log(1f + MaxValue.Blue))),
                MaxValue.Alpha);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        *TargetPointer2 = (byte)(MaxValue.Red * Math.Log(1f + (*TargetPointer2)));
                        ++TargetPointer2;
                        *TargetPointer2 = (byte)(MaxValue.Green * Math.Log(1f + (*TargetPointer2)));
                        ++TargetPointer2;
                        *TargetPointer2 = (byte)(MaxValue.Blue * Math.Log(1f + (*TargetPointer2)));
                        TargetPointer2 += 2;
                    }
                }
            });
            return image;
        }

        private Color GetMaxValue(Image image, Rectangle targetLocation)
        {
            var ReturnValue = new Color(0, 0, 0, 255);
            for (int y = targetLocation.Bottom; y < targetLocation.Top; ++y)
            {
                for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                {
                    byte Red = image.Pixels[((y * image.Width) + x) * 4];
                    if (ReturnValue.Red < Red)
                        ReturnValue.Red = Red;
                    byte Green = image.Pixels[(((y * image.Width) + x) * 4) + 1];
                    if (ReturnValue.Green < Green)
                        ReturnValue.Green = Green;
                    byte Blue = image.Pixels[(((y * image.Width) + x) * 4) + 2];
                    if (ReturnValue.Blue < Blue)
                        ReturnValue.Blue = Blue;
                    if (ReturnValue == Color.White)
                        return ReturnValue;
                }
            }
            return ReturnValue;
        }
    }
}