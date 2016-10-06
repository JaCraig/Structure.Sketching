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
    /// Flips the image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Flip : AffineBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flip"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="filter">The filter.</param>
        public Flip(FlipDirection direction, ResamplingFiltersAvailable filter = ResamplingFiltersAvailable.NearestNeighbor)
            : base(filter: filter)
        {
            Direction = direction;
        }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public FlipDirection Direction { get; set; }

        protected override Matrix3x2 GetMatrix(Image image, Rectangle targetLocation)
        {
            float XScale = 1f;
            float YScale = 1f;
            XScale = (FlipDirection.Horizontal & Direction) == FlipDirection.Horizontal ? -XScale : XScale;
            YScale = (FlipDirection.Vertical & Direction) == FlipDirection.Vertical ? -YScale : YScale;
            return Matrix3x2.CreateScale(XScale, YScale, targetLocation.Center);
        }
    }
}