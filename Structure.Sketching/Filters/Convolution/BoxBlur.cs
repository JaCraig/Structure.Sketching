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

using Structure.Sketching.Filters.Convolution.BaseClasses;

namespace Structure.Sketching.Filters.Convolution
{
    /// <summary>
    /// Box blur convolution filter
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Convolution.BaseClasses.ConvolutionBaseClass" />
    public class BoxBlur : ConvolutionBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxBlur"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public BoxBlur(int size)
        {
            _Height = size;
            _Width = size;
            _Matrix = new float[size * size];
            for (int x = 0; x < _Matrix.Length; ++x)
                _Matrix[x] = 1;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ConvolutionBaseClass"/> is absolute.
        /// </summary>
        /// <value><c>true</c> if absolute; otherwise, <c>false</c>.</value>
        public override bool Absolute => false;

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public override int Height => _Height;

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <value>The matrix.</value>
        public override float[] Matrix => _Matrix;

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public override float Offset => 0;

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public override int Width => _Width;

        private int _Height;
        private float[] _Matrix;
        private int _Width;
    }
}