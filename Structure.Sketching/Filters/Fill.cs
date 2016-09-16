﻿/*
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
    /// Fills in a section of an image with a specific color
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Fill : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fill"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        public Fill(Color color)
        {
            Color = color;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color { get; set; }

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
                        *TargetPointer2 = Color.Red;
                        ++TargetPointer2;
                        *TargetPointer2 = Color.Green;
                        ++TargetPointer2;
                        *TargetPointer2 = Color.Blue;
                        ++TargetPointer2;
                        *TargetPointer2 = Color.Alpha;
                        ++TargetPointer2;
                    }
                }
            });
            return image;
        }
    }
}