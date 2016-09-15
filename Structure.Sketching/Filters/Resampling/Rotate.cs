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

namespace Structure.Sketching.Filters.Resampling
{
    /// <summary>
    /// Rotates an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter" />
    public class Rotate : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rotate"/> class.
        /// </summary>
        /// <param name="angle">The angle.</param>
        public Rotate(float angle)
        {
            Angle = -angle * (float)(Math.PI / 180f);
        }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>The angle.</value>
        public float Angle { get; private set; }

        /// <summary>
        /// Applies the resizing filter to the specified image.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>
        /// The image
        /// </returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var Copy = new Vector4[image.Pixels.Length];
            Array.Copy(image.Pixels, Copy, Copy.Length);
            var Rotation = Matrix3x2.CreateRotation(Angle, targetLocation.Center);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* OutputPointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* OutputPointer2 = OutputPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var rotated = Vector2.Transform(new Vector2(x, y), Rotation);
                        var rotatedY = (int)rotated.Y;
                        var rotatedX = (int)rotated.X;
                        *OutputPointer2 = rotatedX >= 0 && rotatedX < image.Width && rotatedY >= 0 && rotatedY < image.Height ?
                                                Copy[(rotatedY * image.Width) + rotatedX] :
                                                new Vector4(0, 0, 0, 1);
                        OutputPointer2++;
                    }
                }
            });
            return image;
        }
    }
}