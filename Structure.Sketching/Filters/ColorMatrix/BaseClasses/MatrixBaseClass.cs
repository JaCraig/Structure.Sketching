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
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.ColorMatrix.BaseClasses
{
    /// <summary>
    /// Color matrix base class
    /// </summary>
    public abstract class MatrixBaseClass : IFilter
    {
        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <value>The matrix.</value>
        public abstract Matrix5x5 Matrix { get; }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static MatrixBaseClass operator *(MatrixBaseClass value1, MatrixBaseClass value2)
        {
            return new ColorMatrix(value1.Matrix * value2.Matrix);
        }

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
                fixed (byte* pointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* pointer2 = pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var TempVector = Matrix * pointer2;
                        *pointer2 = (byte)TempVector.X;
                        ++pointer2;
                        *pointer2 = (byte)TempVector.Y;
                        ++pointer2;
                        *pointer2 = (byte)TempVector.Z;
                        ++pointer2;
                        *pointer2 = (byte)TempVector.W;
                        ++pointer2;
                    }
                }
            });
            return image;
        }
    }
}