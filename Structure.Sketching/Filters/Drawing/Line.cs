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
using System;

namespace Structure.Sketching.Filters.Drawing
{
    /// <summary>
    /// Line drawing item.
    /// </summary>
    /// <seealso cref="ShapeBaseClass"/>
    public class Line : ShapeBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public Line(Color color, int x1, int y1, int x2, int y2)
            : base(color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// Gets or sets the x1.
        /// </summary>
        /// <value>The x1.</value>
        public int X1 { get; set; }

        /// <summary>
        /// Gets or sets the x2.
        /// </summary>
        /// <value>The x2.</value>
        public int X2 { get; set; }

        /// <summary>
        /// Gets or sets the y1.
        /// </summary>
        /// <value>The y1.</value>
        public int Y1 { get; set; }

        /// <summary>
        /// Gets or sets the y2.
        /// </summary>
        /// <value>The y2.</value>
        public int Y2 { get; set; }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns></returns>
        public override Image Apply(Image image, Numerics.Rectangle targetLocation = default(Numerics.Rectangle))
        {
            targetLocation = targetLocation == default(Numerics.Rectangle) ? new Numerics.Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var IsSteep = Math.Abs(Y2 - Y1) > Math.Abs(X2 - X1);
            if (IsSteep)
            {
                var Temp = X1;
                X1 = Y1;
                Y1 = Temp;
                Temp = X2;
                X2 = Y2;
                Y2 = Temp;
            }
            if (X1 > X2)
            {
                var Temp = X1;
                X1 = X2;
                X2 = Temp;
                Temp = Y1;
                Y1 = Y2;
                Y2 = Temp;
            }
            var ChangeX = X2 - X1;
            var ChangeY = Y2 - Y1;
            float Gradiant = ChangeY / (float)ChangeX;
            var XEnd = Round(X1);
            var YEnd = Y1 + Gradiant * (XEnd - X1);
            var XGap = RFPart(X1 + 0.5);
            var XPixel1 = (int)XEnd;
            var YPixel1 = (int)YEnd;
            if (IsSteep)
            {
                Plot(image, YPixel1, XPixel1, (float)(RFPart(YEnd) * XGap), targetLocation);
                Plot(image, YPixel1 + 1, XPixel1, (float)(FractionalPart(YEnd) * XGap), targetLocation);
            }
            else
            {
                Plot(image, XPixel1, YPixel1, (float)(RFPart(YEnd) * XGap), targetLocation);
                Plot(image, XPixel1, YPixel1 + 1, (float)(FractionalPart(YEnd) * XGap), targetLocation);
            }
            var Intery = YEnd + Gradiant;

            XEnd = Round(X2);
            YEnd = Y2 + Gradiant * (XEnd - X2);
            XGap = FractionalPart(X2 + 0.5);
            var XPixel2 = (int)XEnd;
            var YPixel2 = (int)YEnd;
            if (IsSteep)
            {
                Plot(image, YPixel2, XPixel2, (float)(RFPart(YEnd) * XGap), targetLocation);
                Plot(image, YPixel2 + 1, XPixel2, (float)(FractionalPart(YEnd) * XGap), targetLocation);
            }
            else
            {
                Plot(image, XPixel2, YPixel2, (float)(RFPart(YEnd) * XGap), targetLocation);
                Plot(image, XPixel2, YPixel2 + 1, (float)(FractionalPart(YEnd) * XGap), targetLocation);
            }
            if (IsSteep)
            {
                for (int x = XPixel1 + 1; x < XPixel2; ++x)
                {
                    Plot(image, (int)Intery, x, (float)RFPart(Intery), targetLocation);
                    Plot(image, (int)Intery + 1, x, (float)FractionalPart(Intery), targetLocation);
                    Intery = Intery + Gradiant;
                }
            }
            else
            {
                for (int x = XPixel1 + 1; x < XPixel2; ++x)
                {
                    Plot(image, x, (int)Intery, (float)RFPart(Intery), targetLocation);
                    Plot(image, x, (int)Intery + 1, (float)FractionalPart(Intery), targetLocation);
                    Intery = Intery + Gradiant;
                }
            }
            return image;
        }
    }
}