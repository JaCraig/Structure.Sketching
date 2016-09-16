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
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Translate the image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Translate : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Translate"/> class.
        /// </summary>
        /// <param name="xDelta">The x delta.</param>
        /// <param name="yDelta">The y delta.</param>
        public Translate(int xDelta, int yDelta)
        {
            YDelta = yDelta;
            XDelta = xDelta;
        }

        /// <summary>
        /// Gets or sets the x delta.
        /// </summary>
        /// <value>The x delta.</value>
        public int XDelta { get; set; }

        /// <summary>
        /// Gets or sets the y delta.
        /// </summary>
        /// <value>The y delta.</value>
        public int YDelta { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var Result = new byte[image.Pixels.Length];
            Array.Copy(image.Pixels, Result, Result.Length);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* SourcePointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    byte* SourcePointer2 = SourcePointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var TargetY = y + YDelta;
                        var TargetX = x + XDelta;
                        if (TargetY >= 0 && TargetY < image.Height && TargetX >= 0 && TargetX < image.Width)
                        {
                            var TargetStart = ((TargetY * image.Width) + TargetX) * 4;
                            Result[TargetStart] = *SourcePointer2;
                            ++SourcePointer2;
                            Result[TargetStart + 1] = *SourcePointer2;
                            ++SourcePointer2;
                            Result[TargetStart + 2] = *SourcePointer2;
                            ++SourcePointer2;
                            Result[TargetStart + 3] = *SourcePointer2;
                            ++SourcePointer2;
                        }
                        else
                        {
                            SourcePointer2 += 4;
                        }
                    }
                }
            });
            return image.ReCreate(image.Width, image.Height, Result);
        }
    }
}