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

namespace Structure.Sketching.Filters.Overlays
{
    /// <summary>
    /// Adds a glow effect to an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter" />
    public class Glow : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Glow"/> class.
        /// </summary>
        /// <param name="color">The glow color.</param>
        /// <param name="xRadius">The x radius (between 0 and 1).</param>
        /// <param name="yRadius">The y radius (between 0 and 1).</param>
        public Glow(Vector4 color, float xRadius, float yRadius)
        {
            XRadius = xRadius > 0 ? xRadius : 0.5f;
            YRadius = yRadius > 0 ? yRadius : 0.5f;
            Color = color;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Vector4 Color { get; private set; }

        /// <summary>
        /// Gets the x radius.
        /// </summary>
        /// <value>The x radius.</value>
        public float XRadius { get; private set; }

        /// <summary>
        /// Gets the y radius.
        /// </summary>
        /// <value>The y radius.</value>
        public float YRadius { get; private set; }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var TempX = XRadius * image.Width;
            var TempY = YRadius * image.Height;
            var MaxDistance = (float)Math.Sqrt((TempX * TempX) + (TempY * TempY));

            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* Pointer2 = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        float Distance = Vector2.Distance(image.Center, new Vector2(x, y));

                        var Result = Vector4.Lerp(Color, *Pointer2, .5f * (Distance / MaxDistance));

                        *Pointer2 = Vector4.Clamp(*Pointer2 * (1 - Result.W) + (*Pointer2 * Result * Result.W), Vector4.Zero, Vector4.One);
                        ++Pointer2;
                    }
                }
            });
            return image;
        }
    }
}