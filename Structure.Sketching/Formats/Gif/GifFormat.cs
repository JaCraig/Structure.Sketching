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

using Structure.Sketching.Formats.BaseClasses;
using Structure.Sketching.Formats.Interfaces;

namespace Structure.Sketching.Formats.Gif
{
    /// <summary>
    /// Gif format
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.BaseClasses.FormatBase" />
    public class GifFormat : AnimationFormatBase
    {
        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public override FileFormats Format => FileFormats.GIF;

        /// <summary>
        /// Gets the decoder.
        /// </summary>
        /// <value>
        /// The decoder.
        /// </value>
        protected override IDecoder Decoder => new Decoder();

        /// <summary>
        /// Gets the encoder.
        /// </summary>
        /// <value>
        /// The encoder.
        /// </value>
        protected override IEncoder Encoder => new Encoder();
    }
}