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
using System.Numerics;

namespace Structure.Sketching.Filters.Drawing
{
    /// <summary>
    /// Ellipse drawing class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Drawing.BaseClasses.ShapeBaseClass"/>
    public class Ellipse : ShapeBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipse"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="fill">if set to <c>true</c> [fill].</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="center">The center.</param>
        public Ellipse(Color color, bool fill, int width, int height, Vector2 center)
            : base(color)
        {
            Center = center;
            Height = height;
            Width = width;
            Fill = fill;
        }

        /// <summary>
        /// Gets or sets the center.
        /// </summary>
        /// <value>The center.</value>
        public Vector2 Center { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Ellipse"/> is fill.
        /// </summary>
        /// <value><c>true</c> if fill; otherwise, <c>false</c>.</value>
        public bool Fill { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns></returns>
        public override Image Apply(Image image, Numerics.Rectangle targetLocation = default(Numerics.Rectangle))
        {
            targetLocation = targetLocation == default(Numerics.Rectangle) ? new Numerics.Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            if (Fill)
            {
                DrawFilledEllipse(image, targetLocation);
            }
            else
            {
                DrawEllipse(image, targetLocation);
            }
            return image;
        }

        private void DrawEllipse(Image image, Numerics.Rectangle targetLocation)
        {
            int TwoASquare = 2 * Width * Width;
            int TwoBSquare = 2 * Height * Height;
            int x = Width;
            int y = 0;
            var XChange = Height * Height * (1 - 2 * Width);
            var YChange = Width * Width;
            var EllipseError = 0;
            var StoppingX = TwoBSquare * Width;
            var StoppingY = 0;
            while (StoppingX >= StoppingY)
            {
                Plot4EllipsePoints(image, x, y, targetLocation);
                ++y;
                StoppingY += TwoASquare;
                EllipseError += YChange;
                YChange += TwoASquare;
                if (2 * EllipseError + XChange > 0)
                {
                    --x;
                    StoppingX -= TwoBSquare;
                    EllipseError += XChange;
                    XChange += TwoBSquare;
                }
            }
            x = 0;
            y = Height;
            XChange = Height * Height;
            YChange = Width * Width * (1 - 2 * Height);
            EllipseError = 0;
            StoppingX = 0;
            StoppingY = TwoASquare * Height;
            while (StoppingX <= StoppingY)
            {
                Plot4EllipsePoints(image, x, y, targetLocation);
                ++x;
                StoppingX += TwoBSquare;
                EllipseError += XChange;
                XChange += TwoBSquare;
                if (2 * EllipseError + YChange > 0)
                {
                    --y;
                    StoppingY -= TwoASquare;
                    EllipseError += YChange;
                    YChange += TwoASquare;
                }
            }
        }

        private void DrawFilledEllipse(Image image, Numerics.Rectangle targetLocation)
        {
            int hh = Height * Height;
            int ww = Width * Width;
            int hhww = hh * ww;
            int x0 = Width;
            int dx = 0;

            for (int x = -Width; x <= Width; ++x)
                Plot(image, (int)Center.X + x, (int)Center.Y, 1, targetLocation);

            for (int y = 1; y <= Height; ++y)
            {
                int x1 = x0 - (dx - 1);
                for (; x1 > 0; x1--)
                    if (x1 * x1 * hh + y * y * ww <= hhww)
                        break;
                dx = x0 - x1;
                x0 = x1;

                for (int x = -x0; x <= x0; x++)
                {
                    Plot(image, (int)Center.X + x, (int)Center.Y - y, 1, targetLocation);
                    Plot(image, (int)Center.X + x, (int)Center.Y + y, 1, targetLocation);
                }
            }
        }

        /// <summary>
        /// Plots 4 points along the ellipse.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void Plot4EllipsePoints(Image image, int x, int y, Numerics.Rectangle targetLocation)
        {
            Plot(image, (int)(Center.X + x), (int)(Center.Y + y), 1, targetLocation);
            Plot(image, (int)(Center.X - x), (int)(Center.Y + y), 1, targetLocation);
            Plot(image, (int)(Center.X - x), (int)(Center.Y - y), 1, targetLocation);
            Plot(image, (int)(Center.X + x), (int)(Center.Y - y), 1, targetLocation);
        }
    }
}