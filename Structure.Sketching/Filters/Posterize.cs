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
    /// Posterizes an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Posterize : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Posterize" /> class.
        /// </summary>
        /// <param name="divisions">The number of divisions for each channel.</param>
        public Posterize(int divisions)
        {
            if (divisions < 1)
                divisions = 1;
            var Step = 1f / divisions;
            Divisions = new float[divisions + 1];
            Divisions[0] = 0;
            for (int x = 1; x < divisions; ++x)
            {
                Divisions[x] = Divisions[x - 1] + Step;
            }
            Divisions[divisions] = 1;
        }

        /// <summary>
        /// Gets or sets the divisions.
        /// </summary>
        /// <value>
        /// The divisions.
        /// </value>
        public float[] Divisions { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Vector4* Pointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Vector4* OutputPointer = Pointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        (*OutputPointer).X = FindValue((*OutputPointer).X);
                        (*OutputPointer).Y = FindValue((*OutputPointer).Y);
                        (*OutputPointer).Z = FindValue((*OutputPointer).Z);
                        OutputPointer++;
                    }
                }
            });
            return image;
        }

        private float FindValue(float x)
        {
            if (Divisions.Length == 1)
                return Divisions[0];
            for (int z = 1; z < Divisions.Length; ++z)
            {
                if (Divisions[z] > x)
                    return Divisions[z - 1];
            }
            return Divisions[Divisions.Length - 1];
        }
    }
}