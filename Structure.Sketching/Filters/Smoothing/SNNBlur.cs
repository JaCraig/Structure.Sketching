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
using System.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Morphology
{
    /// <summary>
    /// SNN Blur on an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class SNNBlur : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SNNBlur"/> class.
        /// </summary>
        /// <param name="apetureRadius">The apeture radius.</param>
        public SNNBlur(int apetureRadius)
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
            var TempValues = new Vector4[image.Width * image.Height];
            Array.Copy(image.Pixels, TempValues, TempValues.Length);
            int ApetureMin = -ApetureRadius;
            int ApetureMax = ApetureRadius;
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* TargetPointer = &TempValues[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        float RValue = 0;
                        float GValue = 0;
                        float BValue = 0;
                        int NumPixels = 0;
                        for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                        {
                            int TempX1 = x + x2;
                            int TempX2 = x - x2;
                            if (TempX1 >= targetLocation.Left && TempX1 < targetLocation.Right && TempX2 >= targetLocation.Left && TempX2 < targetLocation.Right)
                            {
                                for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                                {
                                    int TempY1 = y + y2;
                                    int TempY2 = y - y2;
                                    if (TempY1 >= targetLocation.Bottom && TempY1 < targetLocation.Top && TempY2 >= targetLocation.Bottom && TempY2 < targetLocation.Top)
                                    {
                                        var TempValue1 = new Vector3(image.Pixels[(y * image.Width) + x].X,
                                            image.Pixels[(y * image.Width) + x].Y,
                                            image.Pixels[(y * image.Width) + x].Z);
                                        var TempValue2 = new Vector3(image.Pixels[(TempY1 * image.Width) + TempX1].X,
                                            image.Pixels[(TempY1 * image.Width) + TempX1].Y,
                                            image.Pixels[(TempY1 * image.Width) + TempX1].Z);
                                        var TempValue3 = new Vector3(image.Pixels[(TempY2 * image.Width) + TempX2].X,
                                            image.Pixels[(TempY2 * image.Width) + TempX2].Y,
                                            image.Pixels[(TempY2 * image.Width) + TempX2].Z);
                                        if (Vector3.Distance(TempValue1, TempValue2) < Vector3.Distance(TempValue1, TempValue3))
                                        {
                                            RValue += TempValue2.X;
                                            GValue += TempValue2.Y;
                                            BValue += TempValue2.Z;
                                        }
                                        else
                                        {
                                            RValue += TempValue3.X;
                                            GValue += TempValue3.Y;
                                            BValue += TempValue3.Z;
                                        }
                                        ++NumPixels;
                                    }
                                }
                            }
                        }
                        TempValues[(y * image.Width) + x].X = RValue / NumPixels;
                        TempValues[(y * image.Width) + x].Y = GValue / NumPixels;
                        TempValues[(y * image.Width) + x].Z = BValue / NumPixels;
                        TempValues[(y * image.Width) + x].W = image.Pixels[(y * image.Width) + x].W;
                    }
                }
            });
            return image.ReCreate(image.Width, image.Height, TempValues);
        }
    }
}