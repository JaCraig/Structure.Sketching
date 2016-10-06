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
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters
{
    /// <summary>
    /// Resizes the canvas
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class ResizeCanvas : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeCanvas"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="options">The options.</param>
        public ResizeCanvas(int width, int height, ResizeOptions options = ResizeOptions.UpperLeft)
        {
            Options = options;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        public ResizeOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            var Final = new byte[Width * Height * 4];
            var XOffset = 0;
            var YOffset = 0;
            if (Options == ResizeOptions.Center)
            {
                XOffset = (image.Width - Width) / 2;
                YOffset = (image.Height - Height) / 2;
            }
            Parallel.For(0, Height, y =>
            {
                if (y + YOffset >= image.Height || y + YOffset < 0)
                    return;
                fixed (byte* InputPointer = &image.Pixels[(y + YOffset) * image.Width * 4])
                {
                    fixed (byte* OutputPointer = &Final[y * Width * 4])
                    {
                        byte* OutputPointer2 = OutputPointer;
                        byte* InputPointer2 = InputPointer;
                        for (int x = 0; x < Width; ++x)
                        {
                            if ((x + XOffset) >= image.Width)
                                break;
                            if ((x + XOffset) < 0)
                            {
                                OutputPointer2 += 4;
                                InputPointer2 += 4;
                            }
                            else
                            {
                                *OutputPointer2 = *(InputPointer2 + (XOffset * 4));
                                ++OutputPointer2;
                                ++InputPointer2;
                                *OutputPointer2 = *(InputPointer2 + (XOffset * 4));
                                ++OutputPointer2;
                                ++InputPointer2;
                                *OutputPointer2 = *(InputPointer2 + (XOffset * 4));
                                ++OutputPointer2;
                                ++InputPointer2;
                                *OutputPointer2 = *(InputPointer2 + (XOffset * 4));
                                ++OutputPointer2;
                                ++InputPointer2;
                            }
                        }
                    }
                }
            });
            return image.ReCreate(Width, Height, Final);
        }
    }
}