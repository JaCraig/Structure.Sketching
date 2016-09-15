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

namespace Structure.Sketching.Formats.Gif
{
    /// <summary>
    /// Gif decoder
    /// </summary>
    /// <seealso cref="DecoderBase{File}" />
    public class Decoder : DecoderBase<Format.File>
    {
        /// <summary>
        /// Gets the size of the header.
        /// </summary>
        /// <value>
        /// The size of the header.
        /// </value>
        public override int HeaderSize => 6;

        /// <summary>
        /// Gets the file extensions.
        /// </summary>
        /// <value>
        /// The file extensions.
        /// </value>
        protected override string[] FileExtensions => new string[] { ".GIF" };

        /// <summary>
        /// Determines whether this instance can decode the specified header.
        /// </summary>
        /// <param name="header">The header data</param>
        /// <returns>
        /// True if it can, false otherwise
        /// </returns>
        public override bool CanDecode(byte[] header)
        {
            if (header == null || header.Length < 6)
                return false;
            return header[0] == 0x47
                && header[1] == 0x49
                && header[2] == 0x46
                && header[3] == 0x38
                && (header[4] == 0x39 || header[4] == 0x37)
                && header[5] == 0x61;
        }
    }
}