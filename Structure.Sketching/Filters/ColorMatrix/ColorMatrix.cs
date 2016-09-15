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

using Structure.Sketching.Filters.ColorMatrix.BaseClasses;
using Structure.Sketching.Numerics;

namespace Structure.Sketching.Filters.ColorMatrix
{
    /// <summary>
    /// Generic ColorMatrix
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.ColorMatrix.BaseClasses.MatrixBaseClass"/>
    public class ColorMatrix : MatrixBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorMatrix"/> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        public ColorMatrix(Matrix5x5 matrix)
        {
            _Matrix = matrix;
        }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <value>The matrix.</value>
        public override Matrix5x5 Matrix => _Matrix;

        private Matrix5x5 _Matrix;
    }
}