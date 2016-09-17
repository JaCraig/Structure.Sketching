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
using Structure.Sketching.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Morphology
{
    /// <summary>
    /// Medians an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Median : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Median"/> class.
        /// </summary>
        /// <param name="apetureRadius">The apeture radius.</param>
        public Median(int apetureRadius)
        {
            ApetureRadius = apetureRadius;
        }

        /// <summary>
        /// Gets or sets the apeture radius.
        /// </summary>
        /// <value>The apeture radius.</value>
        public int ApetureRadius { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var TempValues = new byte[image.Width * image.Height * 4];
            Array.Copy(image.Pixels, TempValues, TempValues.Length);
            int ApetureMin = -ApetureRadius;
            int ApetureMax = ApetureRadius;
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* TargetPointer = &TempValues[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var RValues = new List<byte>();
                        var GValues = new List<byte>();
                        var BValues = new List<byte>();
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
                                        RValues.Add(image.Pixels[((TempY * image.Width) + TempX) * 4]);
                                        GValues.Add(image.Pixels[(((TempY * image.Width) + TempX) * 4) + 1]);
                                        BValues.Add(image.Pixels[(((TempY * image.Width) + TempX) * 4) + 2]);
                                    }
                                }
                            }
                        }
                        TempValues[((y * image.Width) + x) * 4] = RValues.OrderBy(_ => _).ElementAt(RValues.Count / 2);
                        TempValues[(((y * image.Width) + x) * 4) + 1] = GValues.OrderBy(_ => _).ElementAt(RValues.Count / 2);
                        TempValues[(((y * image.Width) + x) * 4) + 2] = BValues.OrderBy(_ => _).ElementAt(RValues.Count / 2);
                        TempValues[(((y * image.Width) + x) * 4) + 3] = image.Pixels[(((y * image.Width) + x) * 4) + 3];
                    }
                }
            });
            return image.ReCreate(image.Width, image.Height, TempValues);
        }
    }
}