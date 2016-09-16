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

using Structure.Sketching.Colors.ColorSpaces;
using Structure.Sketching.Quantizers.Interfaces;

namespace Structure.Sketching.Quantizers.BaseClasses
{
    /// <summary>
    /// Quantizer base class
    /// </summary>
    /// <seealso cref="IQuantizer" />
    public abstract class QuantizerBase : IQuantizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantizerBase"/> class.
        /// </summary>
        protected QuantizerBase()
        {
        }

        /// <summary>
        /// Gets or sets the transparency threshold.
        /// </summary>
        /// <value>
        /// The transparency threshold.
        /// </value>
        public byte TransparencyThreshold { get; set; }

        /// <summary>
        /// Gets or sets the index of the transparent.
        /// </summary>
        /// <value>
        /// The index of the transparent.
        /// </value>
        public int TransparentIndex { get; protected set; } = -1;

        /// <summary>
        /// Quantizes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxColors">The maximum colors.</param>
        /// <returns>The resulting quantized image</returns>
        public QuantizedImage Quantize(Image image, int maxColors)
        {
            Initialize(image, maxColors);
            Bgra[] Palette = GetPalette();

            byte[] Pixels = Process(image);
            return new QuantizedImage(image.Width, image.Height, Palette, Pixels, TransparentIndex);
        }

        /// <summary>
        /// Gets the palette.
        /// </summary>
        /// <returns>The list of colors in the palette</returns>
        protected abstract Bgra[] GetPalette();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected abstract void Initialize(Image image, int maxColors);

        /// <summary>
        /// Processes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The resulting byte array.</returns>
        protected abstract byte[] Process(Image image);
    }
}