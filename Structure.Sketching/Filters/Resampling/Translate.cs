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
using Structure.Sketching.Filters.Resampling.BaseClasses;
using Structure.Sketching.Numerics;
using System.Numerics;

namespace Structure.Sketching.Filters.Resampling
{
    /// <summary>
    /// Translate the image
    /// </summary>
    /// <seealso cref="AffineBaseClass"/>
    /// <seealso cref="IFilter"/>
    public class Translate : AffineBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Translate"/> class.
        /// </summary>
        /// <param name="xDelta">The x delta.</param>
        /// <param name="yDelta">The y delta.</param>
        public Translate(int xDelta, int yDelta)
        {
            YDelta = -yDelta;
            XDelta = -xDelta;
        }

        /// <summary>
        /// Gets or sets the x delta.
        /// </summary>
        /// <value>The x delta.</value>
        public int XDelta { get; set; }

        /// <summary>
        /// Gets or sets the y delta.
        /// </summary>
        /// <value>The y delta.</value>
        public int YDelta { get; set; }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The matrix used for the transformation</returns>
        protected override Matrix3x2 GetMatrix(Image image, Rectangle targetLocation)
        {
            return Matrix3x2.CreateTranslation(XDelta, YDelta);
        }
    }
}