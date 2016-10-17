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

using Structure.Sketching.Filters.Convolution.Enums;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Effects
{
    /// <summary>
    /// Does a sin wave on an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class SinWave : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SinWave"/> class.
        /// </summary>
        /// <param name="amplitude">The amplitude.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="direction">The direction.</param>
        public SinWave(float amplitude, float frequency, Direction direction)
        {
            Direction = direction;
            Frequency = frequency;
            Amplitude = amplitude;
        }

        /// <summary>
        /// Gets or sets the amplitude.
        /// </summary>
        /// <value>The amplitude.</value>
        public float Amplitude { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public float Frequency { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var Result = new byte[image.Width * image.Height * 4];
            Array.Copy(image.Pixels, Result, Result.Length);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        double Value1 = 0;
                        double Value2 = 0;
                        if (Direction == Direction.RightToLeft || Direction == Direction.LeftToRight)
                            Value1 = System.Math.Sin(((x * Frequency) * System.Math.PI) / 180.0d) * Amplitude;
                        if (Direction == Direction.BottomToTop || Direction == Direction.TopToBottom)
                            Value2 = System.Math.Sin(((y * Frequency) * System.Math.PI) / 180.0d) * Amplitude;
                        Value1 = y - (int)Value1;
                        Value2 = x - (int)Value2;
                        while (Value1 < 0)
                            Value1 += image.Height;
                        while (Value2 < 0)
                            Value2 += image.Width;
                        while (Value1 >= image.Height)
                            Value1 -= image.Height;
                        while (Value2 >= image.Width)
                            Value2 -= image.Width;
                        Result[((y * image.Width) + x) * 4] = image.Pixels[(((int)Value1 * image.Width) + ((int)Value2)) * 4];
                        Result[(((y * image.Width) + x) * 4) + 1] = image.Pixels[((((int)Value1 * image.Width) + ((int)Value2)) * 4) + 1];
                        Result[(((y * image.Width) + x) * 4) + 2] = image.Pixels[((((int)Value1 * image.Width) + ((int)Value2)) * 4) + 2];
                        Result[(((y * image.Width) + x) * 4) + 3] = image.Pixels[((((int)Value1 * image.Width) + ((int)Value2)) * 4) + 3];
                    }
                }
            });
            return image.ReCreate(image.Width, image.Height, Result);
        }
    }
}