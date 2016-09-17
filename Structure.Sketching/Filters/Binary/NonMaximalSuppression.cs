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
using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Binary
{
    /// <summary>
    /// Non-maximal suppression filter
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter" />
    public class NonMaximalSuppression : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonMaximalSuppression" /> class.
        /// </summary>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <param name="threshold1">The threshold1.</param>
        /// <param name="threshold2">The threshold2.</param>
        public NonMaximalSuppression(Color color1, Color color2, float threshold1, float threshold2)
        {
            Threshold2 = threshold2 * 255;
            Threshold1 = threshold1 * 255;
            Color1 = color1;
            Color2 = color2;
        }

        /// <summary>
        /// Gets or sets the color1.
        /// </summary>
        /// <value>The color1.</value>
        public Color Color1 { get; set; }

        /// <summary>
        /// Gets or sets the color2.
        /// </summary>
        /// <value>The color2.</value>
        public Color Color2 { get; set; }

        /// <summary>
        /// Gets or sets the threshold1.
        /// </summary>
        /// <value>
        /// The threshold1.
        /// </value>
        public float Threshold1 { get; set; }

        /// <summary>
        /// Gets or sets the threshold2.
        /// </summary>
        /// <value>
        /// The threshold2.
        /// </value>
        public float Threshold2 { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            new Greyscale709().Apply(image, targetLocation);
            var Result = new Image(image.Width, image.Height, new byte[image.Pixels.Length]);
            Array.Copy(image.Pixels, Result.Pixels, Result.Pixels.Length);
            new Fill(Color2).Apply(Result, targetLocation);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                {
                    if (image.Pixels[((y * image.Width) + x) * 4] >= Threshold1)
                        FillPixels(Result.Pixels, image, x, y, targetLocation);
                }
            });
            return image.ReCreate(image.Width, image.Height, Result.Pixels);
        }

        private void FillPixels(byte[] result, Image image, int x, int y, Rectangle targetLocation)
        {
            var TempPixels = new Stack<Tuple<int, int>>();
            TempPixels.Push(new Tuple<int, int>(x, y));
            while (TempPixels.Count > 0)
            {
                var CurrentPixel = TempPixels.Pop();
                var Left = CurrentPixel.Item1 - 1;
                if (Left >= targetLocation.Left)
                {
                    if (image.Pixels[((CurrentPixel.Item2 * image.Width) + Left) * 4] > Threshold2
                        && new Color(result[((CurrentPixel.Item2 * image.Width) + Left) * 4],
                                    result[(((CurrentPixel.Item2 * image.Width) + Left) * 4) + 1],
                                    result[(((CurrentPixel.Item2 * image.Width) + Left) * 4) + 2],
                                    result[(((CurrentPixel.Item2 * image.Width) + Left) * 4) + 3]) != Color1)
                    {
                        result[((CurrentPixel.Item2 * image.Width) + Left) * 4] = Color1.Red;
                        result[(((CurrentPixel.Item2 * image.Width) + Left) * 4) + 1] = Color1.Green;
                        result[(((CurrentPixel.Item2 * image.Width) + Left) * 4) + 2] = Color1.Blue;
                        result[(((CurrentPixel.Item2 * image.Width) + Left) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(Left, CurrentPixel.Item2));
                    }
                }
                var Right = CurrentPixel.Item1 + 1;
                if (Right < targetLocation.Right)
                {
                    if (image.Pixels[((CurrentPixel.Item2 * image.Width) + Right) * 4] > Threshold2
                        && new Color(result[((CurrentPixel.Item2 * image.Width) + Right) * 4],
                                    result[(((CurrentPixel.Item2 * image.Width) + Right) * 4) + 1],
                                    result[(((CurrentPixel.Item2 * image.Width) + Right) * 4) + 2],
                                    result[(((CurrentPixel.Item2 * image.Width) + Right) * 4) + 3]) != Color1)
                    {
                        result[((CurrentPixel.Item2 * image.Width) + Right) * 4] = Color1.Red;
                        result[(((CurrentPixel.Item2 * image.Width) + Right) * 4) + 1] = Color1.Green;
                        result[(((CurrentPixel.Item2 * image.Width) + Right) * 4) + 2] = Color1.Blue;
                        result[(((CurrentPixel.Item2 * image.Width) + Right) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(Right, CurrentPixel.Item2));
                    }
                }
                var Bottom = CurrentPixel.Item2 - 1;
                if (Bottom >= targetLocation.Bottom)
                {
                    if (image.Pixels[((Bottom * image.Width) + CurrentPixel.Item1) * 4] > Threshold2
                        && new Color(result[((Bottom * image.Width) + CurrentPixel.Item1) * 4],
                                    result[(((Bottom * image.Width) + CurrentPixel.Item1) * 4) + 1],
                                    result[(((Bottom * image.Width) + CurrentPixel.Item1) * 4) + 2]) != Color1)
                    {
                        result[((Bottom * image.Width) + CurrentPixel.Item1) * 4] = Color1.Red;
                        result[(((Bottom * image.Width) + CurrentPixel.Item1) * 4) + 1] = Color1.Green;
                        result[(((Bottom * image.Width) + CurrentPixel.Item1) * 4) + 2] = Color1.Blue;
                        result[(((Bottom * image.Width) + CurrentPixel.Item1) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(CurrentPixel.Item1, Bottom));
                    }
                }
                var Top = CurrentPixel.Item2 + 1;
                if (Top < targetLocation.Top)
                {
                    if (image.Pixels[((Top * image.Width) + CurrentPixel.Item1) * 4] > Threshold2
                        && new Color(result[((Top * image.Width) + CurrentPixel.Item1) * 4],
                                    result[(((Top * image.Width) + CurrentPixel.Item1) * 4) + 1],
                                    result[(((Top * image.Width) + CurrentPixel.Item1) * 4) + 2]) != Color1)
                    {
                        result[((Top * image.Width) + CurrentPixel.Item1) * 4] = Color1.Red;
                        result[(((Top * image.Width) + CurrentPixel.Item1) * 4) + 1] = Color1.Green;
                        result[(((Top * image.Width) + CurrentPixel.Item1) * 4) + 2] = Color1.Blue;
                        result[(((Top * image.Width) + CurrentPixel.Item1) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(CurrentPixel.Item1, Top));
                    }
                }
                if (Left >= targetLocation.Left && Bottom >= targetLocation.Bottom)
                {
                    if (image.Pixels[((Bottom * image.Width) + Left) * 4] > Threshold2
                        && new Color(result[((Bottom * image.Width) + Left) * 4],
                                    result[(((Bottom * image.Width) + Left) * 4) + 1],
                                    result[(((Bottom * image.Width) + Left) * 4) + 2]) != Color1)
                    {
                        result[((Bottom * image.Width) + Left) * 4] = Color1.Red;
                        result[(((Bottom * image.Width) + Left) * 4) + 1] = Color1.Green;
                        result[(((Bottom * image.Width) + Left) * 4) + 2] = Color1.Blue;
                        result[(((Bottom * image.Width) + Left) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(Left, Bottom));
                    }
                }
                if (Left >= targetLocation.Left && Top < targetLocation.Top)
                {
                    if (image.Pixels[((Top * image.Width) + Left) * 4] > Threshold2
                        && new Color(result[((Top * image.Width) + Left) * 4],
                                    result[(((Top * image.Width) + Left) * 4) + 1],
                                    result[(((Top * image.Width) + Left) * 4) + 2]) != Color1)
                    {
                        result[((Top * image.Width) + Left) * 4] = Color1.Red;
                        result[(((Top * image.Width) + Left) * 4) + 1] = Color1.Green;
                        result[(((Top * image.Width) + Left) * 4) + 2] = Color1.Blue;
                        result[(((Top * image.Width) + Left) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(Left, Top));
                    }
                }
                if (Right < targetLocation.Right && Bottom >= targetLocation.Bottom)
                {
                    if (image.Pixels[((Bottom * image.Width) + Right) * 4] > Threshold2
                        && new Color(result[((Bottom * image.Width) + Right) * 4],
                                    result[(((Bottom * image.Width) + Right) * 4) + 1],
                                    result[(((Bottom * image.Width) + Right) * 4) + 2]) != Color1)
                    {
                        result[((Bottom * image.Width) + Right) * 4] = Color1.Red;
                        result[(((Bottom * image.Width) + Right) * 4) + 1] = Color1.Green;
                        result[(((Bottom * image.Width) + Right) * 4) + 2] = Color1.Blue;
                        result[(((Bottom * image.Width) + Right) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(Right, Bottom));
                    }
                }
                if (Right < targetLocation.Left && Top < targetLocation.Top)
                {
                    if (image.Pixels[((Top * image.Width) + Right) * 4] > Threshold2
                        && new Color(result[((Top * image.Width) + Right) * 4],
                                    result[(((Top * image.Width) + Right) * 4) + 1],
                                    result[(((Top * image.Width) + Right) * 4) + 2]) != Color1)
                    {
                        result[((Top * image.Width) + Right) * 4] = Color1.Red;
                        result[(((Top * image.Width) + Right) * 4) + 1] = Color1.Green;
                        result[(((Top * image.Width) + Right) * 4) + 2] = Color1.Blue;
                        result[(((Top * image.Width) + Right) * 4) + 3] = Color1.Alpha;
                        TempPixels.Push(new Tuple<int, int>(Right, Top));
                    }
                }
            }
        }
    }
}