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

using Structure.Sketching.Filters.ColorMatrix.BaseClasses;
using Structure.Sketching.Numerics;

namespace Structure.Sketching.Filters.ColorMatrix
{
    /// <summary>
    /// Polaroid color matrix
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.ColorMatrix.BaseClasses.MatrixBaseClass" />
    public class PolaroidColorMatrix : MatrixBaseClass
    {
        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <value>
        /// The matrix.
        /// </value>
        public override Matrix5x5 Matrix => new Matrix5x5(
            1.538f, -0.062f, -0.262f, 0f, 0f,
            -0.022f, 1.578f, -0.022f, 0f, 0f,
            .216f, -.16f, 1.5831f, 0f, 0f,
            0f, 0f, 0f, 1f, 0f,
            -0.02f, -0.05f, -0.05f, 0f, 1f
        );
    }
}