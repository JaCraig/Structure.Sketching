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
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using Structure.Sketching.Numerics.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Adaptive equalization of an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class AdaptiveEqualize : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveEqualize" /> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="histogram">The histogram.</param>
        public AdaptiveEqualize(int radius, Func<IHistogram> histogram = null)
        {
            Radius = radius;
            Histogram = histogram ?? (() => new RGBHistogram());
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public int Radius { get; set; }

        /// <summary>
        /// Gets or sets the histogram.
        /// </summary>
        /// <value>
        /// The histogram.
        /// </value>
        private Func<IHistogram> Histogram { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var TempValues = new byte[image.Pixels.Length];
            Array.Copy(image.Pixels, TempValues, TempValues.Length);
            int ApetureMin = -Radius;
            int ApetureMax = Radius;
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        List<Color> ColorList = new List<Color>();
                        for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                        {
                            int TempY = y + y2;
                            int TempX = x + ApetureMin;
                            if (TempY >= 0 && TempY < image.Height)
                            {
                                int Length = Radius * 2;
                                if (TempX < 0)
                                {
                                    Length += TempX;
                                    TempX = 0;
                                }
                                var Start = ((TempY * image.Width) + TempX) * 4;
                                fixed (byte* ImagePointer = &TempValues[Start])
                                {
                                    byte* ImagePointer2 = ImagePointer;
                                    for (int x2 = 0; x2 < Length; ++x2)
                                    {
                                        if (TempX >= image.Width)
                                            break;
                                        var TempR = (*ImagePointer2);
                                        ++ImagePointer2;
                                        var TempG = (*ImagePointer2);
                                        ++ImagePointer2;
                                        var TempB = (*ImagePointer2);
                                        ImagePointer2 += 2;
                                        ColorList.Add(new Color(TempR, TempG, TempB));
                                        ++TempX;
                                    }
                                }
                            }
                        }
                        var TempHistogram = Histogram().Load(ColorList.ToArray()).Equalize();

                        var TempR2 = *TargetPointer2;
                        var TempG2 = *(TargetPointer2 + 1);
                        var TempB2 = *(TargetPointer2 + 2);
                        var ResultColor = TempHistogram.EqualizeColor(new Color(TempR2, TempG2, TempB2));
                        (*TargetPointer2) = ResultColor.Red;
                        ++TargetPointer2;
                        (*TargetPointer2) = ResultColor.Green;
                        ++TargetPointer2;
                        (*TargetPointer2) = ResultColor.Blue;
                        TargetPointer2 += 2;
                    }
                }
            });
            return image;
        }
    }
}