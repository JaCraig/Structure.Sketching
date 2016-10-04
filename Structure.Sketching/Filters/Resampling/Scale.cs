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
using Structure.Sketching.Numerics;
using System.Numerics;

namespace Structure.Sketching.Filters.Resampling
{
    /// <summary>
    /// Scales an image to the specified width/height
    /// </summary>
    /// <seealso cref="AffineBaseClass" />
    public class Scale : AffineBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Scale"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Scale(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The transformation matrix</returns>
        protected override Matrix3x2 GetMatrix(Image image, Rectangle targetLocation)
        {
            float XScale = (float)image.Width / Width;
            float YScale = (float)image.Height / Height;
            return Matrix3x2.CreateScale(XScale, YScale, targetLocation.Center);
        }
    }
}