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

namespace Structure.Sketching.Quantizers.Interfaces
{
    /// <summary>
    /// Quantizer interface
    /// </summary>
    public interface IQuantizer
    {
        /// <summary>
        /// Gets or sets the transparency threshold.
        /// </summary>
        /// <value>
        /// The transparency threshold.
        /// </value>
        byte TransparencyThreshold { get; set; }

        /// <summary>
        /// Quantizes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxColors">The maximum colors.</param>
        /// <returns>The resulting quantized image</returns>
        QuantizedImage Quantize(Image image, int maxColors);
    }
}