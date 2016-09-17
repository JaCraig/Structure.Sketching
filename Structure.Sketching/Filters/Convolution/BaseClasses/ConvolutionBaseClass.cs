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
    /// Convolution base class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter" />
    public abstract class ConvolutionBaseClass : IFilter
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
        /// Gets the matrix.
        /// </summary>
        /// <value>The matrix.</value>
        public abstract float[] Matrix { get; }

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
        /// Implements the operator *.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static ConvolutionBaseClass operator *(ConvolutionBaseClass value1, ConvolutionBaseClass value2)
        {
            if (value1.Matrix.Length > value2.Matrix.Length)
            {
                ConvolutionBaseClass TempHolder = value1;
                value1 = value2;
                value2 = TempHolder;
            }
            int Height = value1.Height + value2.Height - 1;
            int Width = value1.Width + value2.Width - 1;
            float[] Values = new float[Width * Height];
            int Value2YPosition = -value2.Height + 1;
            for (int y = 0; y < Height; ++y)
            {
                int Value2XPosition = -value2.Width + 1;
                for (int x = 0; x < Width; ++x)
                {
                    float Value = 0;
                    for (int i = 0; i < value2.Width; ++i)
                    {
                        for (int j = 0; j < value2.Height; ++j)
                        {
                            if (i + Value2XPosition >= 0 && i + Value2XPosition < value1.Width
                                && j + Value2YPosition >= 0 && j + Value2YPosition < value1.Height)
                            {
                                Value += value1.Matrix[(i + Value2XPosition) + ((j + Value2YPosition) * value1.Width)] * value2.Matrix[i + (j * value2.Width)];
                            }
                        }
                    }
                    Values[x + (y * Width)] = Value;
                    ++Value2XPosition;
                }
                ++Value2YPosition;
            }
            return new ConvolutionFilter(Values, Width, Height, value1.Absolute | value2.Absolute, value1.Offset + value2.Offset);
        }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var tempPixels = new byte[image.Pixels.Length];
            Array.Copy(image.Pixels, tempPixels, image.Pixels.Length);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* Pointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* OutputPointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var Values = new Vector4(0, 0, 0, 0);
                        float Weight = 0;
                        int XCurrent = -Width >> 1;
                        int YCurrent = -Height >> 1;
                        int Start = 0;
                        fixed (float* MatrixPointer = &Matrix[0])
                        {
                            float* MatrixValue = MatrixPointer;
                            for (int MatrixIndex = 0; MatrixIndex < Matrix.Length; ++MatrixIndex)
                            {
                                if (MatrixIndex % Width == 0 && MatrixIndex != 0)
                                {
                                    ++YCurrent;
                                    XCurrent = 0;
                                }
                                if (XCurrent + x < image.Width && XCurrent + x >= 0
                                    && YCurrent + y < image.Height && YCurrent + y >= 0)
                                {
                                    if (*MatrixValue != 0)
                                    {
                                        Start = (((YCurrent + y) * image.Width) + (x + XCurrent)) * 4;
                                        Values = Values + new Vector4((*MatrixValue * tempPixels[Start]),
                                                                            (*MatrixValue * tempPixels[Start + 1]),
                                                                            (*MatrixValue * tempPixels[Start + 2]),
                                                                            (*MatrixValue * tempPixels[Start + 3]));
                                        Weight += *MatrixValue;
                                    }
                                    ++MatrixValue;
                                }
                                ++XCurrent;
                            }
                        }
                        if (Weight == 0)
                            Weight = 1;
                        if (Weight > 0)
                        {
                            if (Absolute)
                            {
                                Values = Vector4.Abs(Values);
                            }
                            Values /= Weight;
                            Values = new Vector4(Values.X + Offset, Values.Y + Offset, Values.Z + Offset, 1);
                            Values = Vector4.Clamp(Values, Vector4.Zero, new Vector4(255, 255, 255, 255));
                            *OutputPointer = (byte)Values.X;
                            ++OutputPointer;
                            *OutputPointer = (byte)Values.Y;
                            ++OutputPointer;
                            *OutputPointer = (byte)Values.Z;
                            OutputPointer += 2;
                        }
                        else
                        {
                            OutputPointer += 4;
                        }
                    }
                }
            });
            return image;
        }
    }
}