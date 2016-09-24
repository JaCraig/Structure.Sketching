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
using System;

namespace Structure.Sketching.Filters.ColorMatrix
{
    /// <summary>
    /// Hue matrix
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.ColorMatrix.BaseClasses.MatrixBaseClass" />
    public class Hue : MatrixBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Alpha"/> class.
        /// </summary>
        /// <param name="value">The angle value (0 to 360).</param>
        public Hue(float value)
        {
            value = value * (float)(Math.PI / 180f);
            Value = value;
            var cosradians = Math.Cos(value);
            var sinradians = Math.Sin(value);

            float lumR = .213f;
            float lumG = .715f;
            float lumB = .072f;

            float oneMinusLumR = 1f - lumR;
            float oneMinusLumG = 1f - lumG;
            float oneMinusLumB = 1f - lumB;
            _Matrix = new Matrix5x5
            {
                M11 = (float)(lumR + (cosradians * oneMinusLumR) - (sinradians * lumR)),
                M12 = (float)(lumR - (cosradians * lumR) - (sinradians * 0.143)),
                M13 = (float)(lumR - (cosradians * lumR) - (sinradians * oneMinusLumR)),
                M21 = (float)(lumG - (cosradians * lumG) - (sinradians * lumG)),
                M22 = (float)(lumG + (cosradians * oneMinusLumG) + (sinradians * 0.140)),
                M23 = (float)(lumG - (cosradians * lumG) + (sinradians * lumG)),
                M31 = (float)(lumB - (cosradians * lumB) + (sinradians * oneMinusLumB)),
                M32 = (float)(lumB - (cosradians * lumB) - (sinradians * 0.283)),
                M33 = (float)(lumB + (cosradians * oneMinusLumB) + (sinradians * lumB)),
                M44 = 1,
                M55 = 1
            };
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