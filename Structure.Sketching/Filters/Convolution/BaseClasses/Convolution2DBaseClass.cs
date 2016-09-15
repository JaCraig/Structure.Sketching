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

namespace Structure.Sketching.Filters.Convolution.BaseClasses
{
    /// <summary>
    /// Convolution 2d base class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter" />
    public abstract class Convolution2DBaseClass : IFilter
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ConvolutionBaseClass"/> is absolute.
        /// </summary>
        /// <value><c>true</c> if absolute; otherwise, <c>false</c>.</value>
        public abstract bool Absolute { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public abstract int Height { get; }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public abstract float Offset { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public abstract int Width { get; }

        /// <summary>
        /// Gets the x matrix.
        /// </summary>
        /// <value>The x matrix.</value>
        public abstract float[] XMatrix { get; }

        /// <summary>
        /// Gets the y matrix.
        /// </summary>
        /// <value>The y matrix.</value>
        public abstract float[] YMatrix { get; }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);

            var tempPixels = new Vector4[image.Pixels.Length];
            Array.Copy(image.Pixels, tempPixels, image.Pixels.Length);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* OutputPointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var XValue = new Vector4(0, 0, 0, 0);
                        var YValue = new Vector4(0, 0, 0, 0);
                        float WeightX = 0;
                        float WeightY = 0;
                        int XCurrent = -Width >> 1;
                        int YCurrent = -Height >> 1;
                        int Start = 0;
                        fixed (float* XMatrixPointer = &XMatrix[0])
                        {
                            fixed (float* YMatrixPointer = &YMatrix[0])
                            {
                                float* XMatrixValue = XMatrixPointer;
                                float* YMatrixValue = YMatrixPointer;
                                for (int MatrixIndex = 0; MatrixIndex < XMatrix.Length; ++MatrixIndex)
                                {
                                    if (MatrixIndex % Width == 0 && MatrixIndex != 0)
                                    {
                                        ++YCurrent;
                                        XCurrent = 0;
                                    }
                                    if (XCurrent + x < image.Width && XCurrent + x >= 0
                                        && YCurrent + y < image.Height && YCurrent + y >= 0)
                                    {
                                        if (*XMatrixValue != 0 || *YMatrixValue != 0)
                                        {
                                            Start = ((YCurrent + y) * image.Width) + (x + XCurrent);
                                            XValue = XValue + (*XMatrixValue * tempPixels[Start]);
                                            YValue = YValue + (*YMatrixValue * tempPixels[Start]);
                                            WeightX += *XMatrixValue;
                                            WeightY += *YMatrixValue;
                                        }
                                        ++XMatrixValue;
                                        ++YMatrixValue;
                                    }
                                    ++XCurrent;
                                }
                            }
                        }
                        if (WeightX == 0)
                            WeightX = 1;
                        if (WeightY == 0)
                            WeightY = 1;
                        if (WeightX > 0 && WeightY > 0)
                        {
                            if (Absolute)
                            {
                                YValue = Vector4.Abs(YValue);
                                XValue = Vector4.Abs(XValue);
                            }
                            XValue /= WeightX;
                            YValue /= WeightY;
                            var TempResult = Vector4.SquareRoot((XValue * XValue) + (YValue * YValue));
                            TempResult.W = (*OutputPointer).W;
                            *OutputPointer = Vector4.Clamp(TempResult, Vector4.Zero, Vector4.One);
                        }
                        ++OutputPointer;
                    }
                }
            });
            return image;
        }
    }
}