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
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Flips the image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Flip : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flip"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public Flip(FlipDirection direction)
        {
            Direction = direction;
        }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public FlipDirection Direction { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            int StartX = targetLocation.Left;
            int StartY = targetLocation.Bottom;
            int EndX = (FlipDirection.Horizontal & Direction) == FlipDirection.Horizontal ? (int)targetLocation.Center.X : targetLocation.Right;
            int EndY = (FlipDirection.Vertical & Direction) == FlipDirection.Vertical ? (int)targetLocation.Center.Y : targetLocation.Top;
            if ((FlipDirection.Vertical & Direction) == FlipDirection.Vertical && (FlipDirection.Horizontal & Direction) == FlipDirection.Horizontal)
            {
                EndY = targetLocation.Top;
            }
            Parallel.For(StartY, EndY, y =>
            {
                fixed (byte* TargetPointer = &image.Pixels[GetPointer(y, targetLocation, image)])
                {
                    byte* TargetPointer2 = TargetPointer;
                    fixed (byte* SourcePointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                    {
                        byte* SourcePointer2 = SourcePointer;
                        for (int x = StartX; x < EndX; ++x)
                        {
                            byte value = *SourcePointer2;
                            *SourcePointer2 = *TargetPointer2;
                            *TargetPointer2 = value;
                            ++SourcePointer2;
                            ++TargetPointer2;
                            value = *SourcePointer2;
                            *SourcePointer2 = *TargetPointer2;
                            *TargetPointer2 = value;
                            ++SourcePointer2;
                            ++TargetPointer2;
                            value = *SourcePointer2;
                            *SourcePointer2 = *TargetPointer2;
                            *TargetPointer2 = value;
                            ++SourcePointer2;
                            ++TargetPointer2;
                            value = *SourcePointer2;
                            *SourcePointer2 = *TargetPointer2;
                            *TargetPointer2 = value;
                            ++SourcePointer2;
                            ++TargetPointer2;
                            if ((FlipDirection.Horizontal & Direction) == FlipDirection.Horizontal)
                                TargetPointer2 -= 8;
                        }
                    }
                }
            });
            return image;
        }

        private int GetPointer(int y, Rectangle targetLocation, Image image)
        {
            int Value = 0;
            if ((FlipDirection.Vertical & Direction) == FlipDirection.Vertical)
            {
                Value = (targetLocation.Top - (y + 1)) * image.Width;
            }
            else
            {
                Value = y * image.Width;
            }
            if ((FlipDirection.Horizontal & Direction) == FlipDirection.Horizontal)
            {
                Value += targetLocation.Right - 1;
            }
            else
            {
                Value += targetLocation.Left;
            }
            return Value * 4;
        }
    }
}