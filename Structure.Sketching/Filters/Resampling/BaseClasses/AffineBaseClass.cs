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

namespace Structure.Sketching.Filters.Resampling.BaseClasses
{
    /// <summary>
    /// Affine transformation base class
    /// </summary>
    /// <seealso cref="IFilter" />
    public abstract class AffineBaseClass : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AffineBaseClass" /> class.
        /// </summary>
        protected AffineBaseClass()
        {
        }

        /// <summary>
        /// Gets the transformation matrix.
        /// </summary>
        /// <value>
        /// The transformation matrix.
        /// </value>
        private Matrix3x2 TransformationMatrix { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>
        /// The image
        /// </returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var Copy = new byte[image.Pixels.Length];
            Array.Copy(image.Pixels, Copy, Copy.Length);
            TransformationMatrix = GetMatrix(image, targetLocation);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* OutputPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* OutputPointer2 = OutputPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var rotated = Vector2.Transform(new Vector2(x, y), TransformationMatrix);
                        var rotatedY = (int)rotated.Y;
                        var rotatedX = (int)rotated.X;
                        if (rotatedX >= 0 && rotatedX < image.Width && rotatedY >= 0 && rotatedY < image.Height)
                        {
                            var CopyIndex = ((rotatedY * image.Width) + rotatedX) * 4;
                            *OutputPointer2 = Copy[CopyIndex];
                            ++OutputPointer2;
                            *OutputPointer2 = Copy[CopyIndex + 1];
                            ++OutputPointer2;
                            *OutputPointer2 = Copy[CopyIndex + 2];
                            ++OutputPointer2;
                            *OutputPointer2 = Copy[CopyIndex + 3];
                            ++OutputPointer2;
                        }
                        else
                        {
                            *OutputPointer2 = 0;
                            ++OutputPointer2;
                            *OutputPointer2 = 0;
                            ++OutputPointer2;
                            *OutputPointer2 = 0;
                            ++OutputPointer2;
                            *OutputPointer2 = 255;
                            ++OutputPointer2;
                        }
                    }
                }
            });
            return image;
        }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>
        /// The matrix used for the transformation
        /// </returns>
        protected abstract Matrix3x2 GetMatrix(Image image, Rectangle targetLocation);
    }
}