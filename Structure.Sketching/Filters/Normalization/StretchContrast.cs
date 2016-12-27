using Structure.Sketching.Colors;
using Structure.Sketching.ExtensionMethods;

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
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Normalization
{
    /// <summary>
    /// Stretches the contrast of a specific image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class StretchContrast : IFilter
    {
        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            byte[] MinValue = new byte[3];
            byte[] MaxValue = new byte[3];
            GetMinMaxPixel(MinValue, MaxValue, image);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Color* TargetPointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Color* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        image.Pixels[(y * image.Width) + x].Red = Map(image.Pixels[(y * image.Width) + x].Red, MinValue[0], MaxValue[0]);
                        image.Pixels[(y * image.Width) + x].Green = Map(image.Pixels[(y * image.Width) + x].Green, MinValue[1], MaxValue[1]);
                        image.Pixels[(y * image.Width) + x].Blue = Map(image.Pixels[(y * image.Width) + x].Blue, MinValue[2], MaxValue[2]);
                    }
                }
            });
            return image;
        }

        private void GetMinMaxPixel(byte[] minValue, byte[] maxValue, Image image)
        {
            minValue[0] = minValue[1] = minValue[2] = 255;
            maxValue[0] = maxValue[1] = maxValue[2] = 0;
            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    var TempR = image.Pixels[(y * image.Width) + x].Red;
                    var TempG = image.Pixels[(y * image.Width) + x].Green;
                    var TempB = image.Pixels[(y * image.Width) + x].Blue;
                    if (minValue[0] > TempR)
                        minValue[0] = TempR;
                    if (maxValue[0] < TempR)
                        maxValue[0] = TempR;

                    if (minValue[1] > TempG)
                        minValue[1] = TempG;
                    if (maxValue[1] < TempG)
                        maxValue[1] = TempG;

                    if (minValue[2] > TempB)
                        minValue[2] = TempB;
                    if (maxValue[2] < TempB)
                        maxValue[2] = TempB;
                }
            }
        }

        private byte Map(byte v, byte min, byte max)
        {
            float TempVal = v - min;
            TempVal /= max - min;
            TempVal *= 255;
            return (byte)TempVal.Clamp(0, 255);
        }
    }
}