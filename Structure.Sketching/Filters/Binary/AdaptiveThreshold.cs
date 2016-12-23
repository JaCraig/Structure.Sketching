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
using System.Linq;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Binary
{
    /// <summary>
    /// Adaptive threshold an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class AdaptiveThreshold : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveThreshold" /> class.
        /// </summary>
        /// <param name="apetureRadius">The apeture radius.</param>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <param name="threshold">The threshold.</param>
        public AdaptiveThreshold(int apetureRadius, Color color1, Color color2, float threshold)
        {
            Threshold = threshold;
            Color2 = color2;
            Color1 = color1;
            ApetureRadius = apetureRadius;
        }

        /// <summary>
        /// Gets or sets the apeture radius.
        /// </summary>
        /// <value>The apeture radius.</value>
        public int ApetureRadius { get; set; }

        /// <summary>
        /// Gets or sets the color1.
        /// </summary>
        /// <value>
        /// The color1.
        /// </value>
        public Color Color1 { get; set; }

        /// <summary>
        /// Gets or sets the color2.
        /// </summary>
        /// <value>
        /// The color2.
        /// </value>
        public Color Color2 { get; set; }

        /// <summary>
        /// Gets or sets the threshold.
        /// </summary>
        /// <value>
        /// The threshold.
        /// </value>
        public float Threshold { get; set; }

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
            var TempValues = new Color[image.Width * image.Height];
            Array.Copy(image.Pixels, TempValues, TempValues.Length);
            int ApetureMin = -ApetureRadius;
            int ApetureMax = ApetureRadius;
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Color* TargetPointer = &TempValues[(y * image.Width) + targetLocation.Left])
                {
                    Color* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var RValues = new List<byte>();
                        for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= targetLocation.Left && TempX < targetLocation.Right)
                            {
                                for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= targetLocation.Bottom && TempY < targetLocation.Top)
                                    {
                                        RValues.Add(image.Pixels[(TempY * image.Width) + TempX].Red);
                                    }
                                }
                            }
                        }
                        *TargetPointer2 = RValues.Average(_ => _ / 255f) >= Threshold ? Color1 : Color2;
                        ++TargetPointer2;
                    }
                }
            });
            return image.ReCreate(image.Width, image.Height, TempValues);
        }
    }
}