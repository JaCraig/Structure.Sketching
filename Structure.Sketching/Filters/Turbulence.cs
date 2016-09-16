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

using Structure.Sketching.ExtensionMethods;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using Structure.Sketching.Procedural;
using System;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Adds turbulence to an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Turbulence : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SinWave"/> class.
        /// </summary>
        /// <param name="roughness">The roughness.</param>
        /// <param name="power">The power.</param>
        /// <param name="seed">The seed.</param>
        public Turbulence(int roughness = 8, float power = 0.02f, int seed = 25123864)
        {
            Seed = seed;
            Power = power;
            Roughness = roughness;
        }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        /// <value>The power.</value>
        public float Power { get; set; }

        /// <summary>
        /// Gets or sets the roughness.
        /// </summary>
        /// <value>The roughness.</value>
        public int Roughness { get; set; }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        /// <value>The seed.</value>
        public int Seed { get; set; }

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
            Image XNoise = PerlinNoise.Generate(image.Width, image.Height, 255, 0, 0.0625f, 1.0f, 0.5f, Roughness, Seed);
            Image YNoise = PerlinNoise.Generate(image.Width, image.Height, 255, 0, 0.0625f, 1.0f, 0.5f, Roughness, Seed * 2);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        float XDistortion = x + (XNoise.Pixels[((y * image.Width) + x) * 4] * Power);
                        float YDistortion = y + (YNoise.Pixels[((y * image.Width) + x) * 4] * Power);
                        var X1 = (int)XDistortion.Clamp(0, image.Width - 1);
                        var Y1 = (int)YDistortion.Clamp(0, image.Height - 1);
                        int ResultOffset = ((y * image.Width) + x) * 4;
                        int SourceOffset = ((Y1 * image.Width) + X1) * 4;

                        Result[ResultOffset] = image.Pixels[SourceOffset];
                        Result[ResultOffset + 1] = image.Pixels[SourceOffset + 1];
                        Result[ResultOffset + 2] = image.Pixels[SourceOffset + 2];
                        Result[ResultOffset + 3] = image.Pixels[SourceOffset + 3];
                    }
                }
            });
            return image.ReCreate(image.Width, image.Height, Result);
        }
    }
}