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

using Structure.Sketching.Colors;
using Structure.Sketching.Filters.Drawing.BaseClasses;

namespace Structure.Sketching.Filters.Drawing
{
    /// <summary>
    /// Rectangle drawing class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Drawing.BaseClasses.ShapeBaseClass"/>
    public class Rectangle : ShapeBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="fill">if set to <c>true</c> [fill].</param>
        /// <param name="bounds">The bounds.</param>
        public Rectangle(Color color, bool fill, Numerics.Rectangle bounds)
            : base(color)
        {
            Bounds = bounds;
            Fill = fill;
        }

        /// <summary>
        /// Gets or sets the bounds.
        /// </summary>
        /// <value>The bounds.</value>
        public Numerics.Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Rectangle"/> is fill.
        /// </summary>
        /// <value><c>true</c> if fill; otherwise, <c>false</c>.</value>
        public bool Fill { get; set; }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns></returns>
        public override Image Apply(Image image, Numerics.Rectangle targetLocation = default(Numerics.Rectangle))
        {
            targetLocation = targetLocation == default(Numerics.Rectangle) ? new Numerics.Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            Bounds = Bounds.Clamp(targetLocation);
            if (Fill)
            {
                return new Fill(Color).Apply(image, Bounds);
            }
            new Line(Color, Bounds.Left, Bounds.Bottom, Bounds.Right, Bounds.Bottom).Apply(image, targetLocation);
            new Line(Color, Bounds.Left, Bounds.Top, Bounds.Right, Bounds.Top).Apply(image, targetLocation);
            new Line(Color, Bounds.Left, Bounds.Bottom, Bounds.Left, Bounds.Top).Apply(image, targetLocation);
            new Line(Color, Bounds.Right, Bounds.Bottom, Bounds.Right, Bounds.Top).Apply(image, targetLocation);
            return image;
        }
    }
}