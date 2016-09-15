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

using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Replaces a color with another color in the image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter" />
    public class Replace : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Replace"/> class.
        /// </summary>
        /// <param name="sourceColor">Color in the image to replace.</param>
        /// <param name="targetColor">Color to replace the sourceColor with.</param>
        /// <param name="epsilon">The epsilon.</param>
        public Replace(Vector4 sourceColor, Vector4 targetColor, float epsilon)
        {
            Epsilon = epsilon;
            TargetColor = targetColor;
            SourceColor = sourceColor;
        }

        /// <summary>
        /// Gets or sets the epsilon.
        /// </summary>
        /// <value>The epsilon.</value>
        public float Epsilon { get; set; }

        /// <summary>
        /// Gets or sets the color of the source.
        /// </summary>
        /// <value>The color of the source.</value>
        public Vector4 SourceColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the target.
        /// </summary>
        /// <value>The color of the target.</value>
        public Vector4 TargetColor { get; set; }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* OutputPointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        if (Vector4.Abs(*OutputPointer - SourceColor).Length() < Epsilon)
                        {
                            *(OutputPointer++) = TargetColor;
                        }
                    }
                }
            });
            return image;
        }
    }
}