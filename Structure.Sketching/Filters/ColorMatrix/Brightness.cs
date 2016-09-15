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
    /// Brightness matrix
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.ColorMatrix.BaseClasses.MatrixBaseClass" />
    public class Brightness : MatrixBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Brightness"/> class.
        /// </summary>
        /// <param name="value">The brightness value (-1 to 1).</param>
        public Brightness(float value)
        {
            Value = value;
            _Matrix = new Matrix5x5
            (
                1f, 0f, 0f, 0f, 0f,
                0f, 1f, 0f, 0f, 0f,
                0f, 0f, 1f, 0f, 0f,
                0f, 0f, 0f, 1f, 0f,
                value, value, value, 0f, 1f
            );
        }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <value>The matrix.</value>
        public override Matrix5x5 Matrix => _Matrix;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public float Value { get; }

        /// <summary>
        /// The matrix backing field
        /// </summary>
        private Matrix5x5 _Matrix = new Matrix5x5
        (
            1f, 0f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f, 0f,
            0f, 0f, 1f, 0f, 0f,
            0f, 0f, 0f, 1f, 0f,
            0f, 0f, 0f, 0f, 1f
        );
    }
}