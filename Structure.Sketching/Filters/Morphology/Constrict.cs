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
    /// Constricts an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Constrict : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Constrict"/> class.
        /// </summary>
        /// <param name="apetureRadius">The apeture radius.</param>
        public Constrict(int apetureRadius)
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
            var TempValues = new Vector4[image.Pixels.Length];
            Array.Copy(image.Pixels, TempValues, TempValues.Length);
            int ApetureMin = -ApetureRadius;
            int ApetureMax = ApetureRadius;
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* TargetPointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        float RValue = float.MaxValue;
                        float GValue = float.MaxValue;
                        float BValue = float.MaxValue;
                        for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                        {
                            int TempY = y + y2;
                            int TempX = x + ApetureMin;
                            if (TempY >= 0 && TempY < image.Height)
                            {
                                int Length = ApetureRadius * 2;
                                if (TempX < 0)
                                {
                                    Length += TempX;
                                    TempX = 0;
                                }
                                var Start = (TempY * image.Width) + TempX;
                                fixed (Vector4* ImagePointer = &TempValues[Start])
                                {
                                    Vector4* ImagePointer2 = ImagePointer;
                                    for (int x2 = 0; x2 < Length; ++x2)
                                    {
                                        if (TempX >= image.Width)
                                            break;
                                        var TempR = (*ImagePointer2).X;
                                        var TempG = (*ImagePointer2).Y;
                                        var TempB = (*ImagePointer2).Z;
                                        ImagePointer2++;
                                        RValue = RValue < TempR ? RValue : TempR;
                                        GValue = GValue < TempG ? GValue : TempG;
                                        BValue = BValue < TempB ? BValue : TempB;
                                        ++TempX;
                                    }
                                }
                            }
                        }
                        (*TargetPointer2).X = RValue;
                        (*TargetPointer2).Y = GValue;
                        (*TargetPointer2).Z = BValue;
                        TargetPointer2++;
                    }
                }
            });
            return image;
        }
    }
}