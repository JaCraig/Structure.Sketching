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
using System;
using System.Numerics;
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
            MaxValue = new Vector4((float)(1d / Math.Log(1f + MaxValue.X)),
                (float)(1d / Math.Log(1f + MaxValue.Y)),
                (float)(1d / Math.Log(1f + MaxValue.Z)),
                MaxValue.W);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* TargetPointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        *TargetPointer2 = new Vector4((float)(MaxValue.X * Math.Log(1f + (*TargetPointer2).X)),
                            (float)(MaxValue.Y * Math.Log(1f + (*TargetPointer2).Y)),
                            (float)(MaxValue.Z * Math.Log(1f + (*TargetPointer2).Z)),
                            (*TargetPointer2).W);
                        ++TargetPointer2;
                    }
                }
            });
            return image;
        }

        private Vector4 GetMaxValue(Image image, Rectangle targetLocation)
        {
            var ReturnValue = new Vector4(0, 0, 0, 1);
            for (int y = targetLocation.Bottom; y < targetLocation.Top; ++y)
            {
                for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                {
                    Vector4 Pixel = image.Pixels[(y * image.Width) + x];
                    if (ReturnValue.X < Pixel.X)
                        ReturnValue.X = Pixel.X;
                    if (ReturnValue.Y < Pixel.Y)
                        ReturnValue.Y = Pixel.Y;
                    if (ReturnValue.Z < Pixel.Z)
                        ReturnValue.Z = Pixel.Z;
                    if (ReturnValue == Vector4.One)
                        return ReturnValue;
                }
            }
            return ReturnValue;
        }
    }
}