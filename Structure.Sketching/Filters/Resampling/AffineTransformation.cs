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

using Structure.Sketching.Filters.Resampling.BaseClasses;
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Numerics;
using System.Numerics;

namespace Structure.Sketching.Filters.Resampling
{
    /// <summary>
    /// Affine transformation
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Resampling.BaseClasses.AffineBaseClass"/>
    public class AffineTransformation : AffineBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AffineTransformation"/> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        /// <param name="filter">The filter to use (defaults to nearest neighbor).</param>
        public AffineTransformation(Matrix3x2 matrix, int width = -1, int height = -1, ResamplingFiltersAvailable filter = ResamplingFiltersAvailable.NearestNeighbor)
            : base(width, height, filter)
        {
            Matrix = matrix;
        }

        /// <summary>
        /// Gets or sets the matrix.
        /// </summary>
        /// <value>The matrix.</value>
        public Matrix3x2 Matrix { get; set; }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static AffineTransformation operator *(AffineTransformation value1, AffineTransformation value2)
        {
            return new AffineTransformation(value1.Matrix * value2.Matrix);
        }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The matrix used for the transformation</returns>
        protected override Matrix3x2 GetMatrix(Image image, Rectangle targetLocation)
        {
            return Matrix;
        }
    }
}