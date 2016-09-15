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
using System.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
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
            Vector3 MinValue;
            Vector3 MaxValue;
            GetMinMaxPixel(out MinValue, out MaxValue, image);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* TargetPointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        image.Pixels[(y * image.Width) + x].X = Map(image.Pixels[(y * image.Width) + x].X, MinValue.X, MaxValue.X);
                        image.Pixels[(y * image.Width) + x].Y = Map(image.Pixels[(y * image.Width) + x].Y, MinValue.Y, MaxValue.Y);
                        image.Pixels[(y * image.Width) + x].Z = Map(image.Pixels[(y * image.Width) + x].Z, MinValue.Z, MaxValue.Z);
                    }
                }
            });
            return image;
        }

        private void GetMinMaxPixel(out Vector3 minValue, out Vector3 maxValue, Image image)
        {
            float MinR = 1, MinG = 1, MinB = 1;
            float MaxR = 0, MaxG = 0, MaxB = 0;
            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    var TempValue = new Vector3(image.Pixels[(y * image.Width) + x].X,
                        image.Pixels[(y * image.Width) + x].Y,
                        image.Pixels[(y * image.Width) + x].Z);
                    if (MinR > TempValue.X)
                        MinR = TempValue.X;
                    if (MaxR < TempValue.X)
                        MaxR = TempValue.X;

                    if (MinG > TempValue.Y)
                        MinG = TempValue.Y;
                    if (MaxG < TempValue.Y)
                        MaxG = TempValue.Y;

                    if (MinB > TempValue.Z)
                        MinB = TempValue.Z;
                    if (MaxB < TempValue.Z)
                        MaxB = TempValue.Z;
                }
            }
            minValue = new Vector3(MinR, MinG, MinB);
            maxValue = new Vector3(MaxR, MaxG, MaxB);
        }

        private float Map(float v, float min, float max)
        {
            float TempVal = v - min;
            TempVal /= max - min;
            return TempVal.Clamp(0, 1);
        }
    }
}