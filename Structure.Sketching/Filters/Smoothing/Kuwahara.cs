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

namespace Structure.Sketching.Filters.Smoothing
{
    /// <summary>
    /// Does smoothing using Kuwahara filter on an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Kuwahara : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Kuwahara"/> class.
        /// </summary>
        /// <param name="apetureRadius">The apeture radius.</param>
        public Kuwahara(int apetureRadius)
        {
            ApetureRadius = apetureRadius;
            ApetureMinX = new int[] { -ApetureRadius, 0, -ApetureRadius, 0 };
            ApetureMaxX = new int[] { 0, ApetureRadius, 0, ApetureRadius };
            ApetureMinY = new int[] { -ApetureRadius, -ApetureRadius, 0, 0 };
            ApetureMaxY = new int[] { 0, 0, ApetureRadius, ApetureRadius };
        }

        /// <summary>
        /// Gets or sets the apeture radius.
        /// </summary>
        /// <value>The apeture radius.</value>
        public int ApetureRadius { get; set; }

        private readonly int[] ApetureMaxX;
        private readonly int[] ApetureMaxY;
        private readonly int[] ApetureMinX;
        private readonly int[] ApetureMinY;

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            Vector4[] Result = new Vector4[image.Width * image.Height];
            Array.Copy(image.Pixels, Result, Result.Length);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* SourcePointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        float[] RValues = { 0, 0, 0, 0 };
                        float[] GValues = { 0, 0, 0, 0 };
                        float[] BValues = { 0, 0, 0, 0 };
                        float[] NumPixels = { 0, 0, 0, 0 };
                        float[] MaxRValue = { 0, 0, 0, 0 };
                        float[] MaxGValue = { 0, 0, 0, 0 };
                        float[] MaxBValue = { 0, 0, 0, 0 };
                        float[] MinRValue = { 1, 1, 1, 1 };
                        float[] MinGValue = { 1, 1, 1, 1 };
                        float[] MinBValue = { 1, 1, 1, 1 };

                        for (int i = 0; i < 4; ++i)
                        {
                            for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
                            {
                                int TempX = x + x2;
                                if (TempX >= 0 && TempX < image.Width)
                                {
                                    for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
                                    {
                                        int TempY = y + y2;
                                        if (TempY >= 0 && TempY < image.Height)
                                        {
                                            RValues[i] += image.Pixels[(TempY * image.Width) + TempX].X;
                                            GValues[i] += image.Pixels[(TempY * image.Width) + TempX].Y;
                                            BValues[i] += image.Pixels[(TempY * image.Width) + TempX].Z;

                                            if (image.Pixels[(TempY * image.Width) + TempX].X > MaxRValue[i])
                                                MaxRValue[i] = image.Pixels[(TempY * image.Width) + TempX].X;
                                            else if (image.Pixels[(TempY * image.Width) + TempX].X < MinRValue[i])
                                                MinRValue[i] = image.Pixels[(TempY * image.Width) + TempX].X;

                                            if (image.Pixels[(TempY * image.Width) + TempX].Y > MaxGValue[i])
                                                MaxGValue[i] = image.Pixels[(TempY * image.Width) + TempX].Y;
                                            else if (image.Pixels[(TempY * image.Width) + TempX].Y < MinGValue[i])
                                                MinGValue[i] = image.Pixels[(TempY * image.Width) + TempX].Y;

                                            if (image.Pixels[(TempY * image.Width) + TempX].Z > MaxBValue[i])
                                                MaxBValue[i] = image.Pixels[(TempY * image.Width) + TempX].Z;
                                            else if (image.Pixels[(TempY * image.Width) + TempX].Z < MinBValue[i])
                                                MinBValue[i] = image.Pixels[(TempY * image.Width) + TempX].Z;

                                            ++NumPixels[i];
                                        }
                                    }
                                }
                            }
                        }

                        int j = 0;
                        float MinDifference = float.MaxValue;
                        for (int i = 0; i < 4; ++i)
                        {
                            float CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) + (MaxBValue[i] - MinBValue[i]);
                            if (CurrentDifference < MinDifference && NumPixels[i] > 0)
                            {
                                j = i;
                                MinDifference = CurrentDifference;
                            }
                        }
                        RValues[j] = RValues[j] / NumPixels[j];
                        GValues[j] = GValues[j] / NumPixels[j];
                        BValues[j] = BValues[j] / NumPixels[j];

                        Result[(y * image.Width) + x] = new Vector4(RValues[j], GValues[j], BValues[j], image.Pixels[(y * image.Width) + x].W);
                    }
                }
            });
            return image.ReCreate(image.Width, image.Height, Result);
        }
    }
}