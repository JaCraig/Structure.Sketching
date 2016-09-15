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

namespace Structure.Sketching.Formats.Jpeg
{
    /// <summary>
    /// JPEG decoder
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
        public override int HeaderSize => 11;

        /// <summary>
        /// Gets the file extensions.
        /// </summary>
        /// <value>
        /// The file extensions.
        /// </value>
        protected override string[] FileExtensions => new string[] { ".JPEG", ".JPG", ".JPE", ".JIF", ".JFIF", ".JFI" };

        /// <summary>
        /// Determines whether this instance can decode the specified header.
        /// </summary>
        /// <param name="header">The header data</param>
        /// <returns>
        /// True if it can, false otherwise
        /// </returns>
        public override bool CanDecode(byte[] header)
        {
            if (header == null || header.Length < 11)
                return false;
            return IsJpeg(header) || IsJfif(header) || IsExif(header);
        }

        /// <summary>
        /// Determines whether the specified header is exif.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>True if it is, false otherwise</returns>
        private static bool IsExif(byte[] header)
        {
            return header[6] == 0x45 && header[7] == 0x78
                && header[8] == 0x69 && header[9] == 0x66
                && header[10] == 0x00;
        }

        /// <summary>
        /// Determines whether the specified header is jfif.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>True if it is, false otherwise.</returns>
        private static bool IsJfif(byte[] header)
        {
            return header[6] == 0x4A && header[7] == 0x46
                && header[8] == 0x49 && header[9] == 0x46
                && header[10] == 0x00;
        }

        /// <summary>
        /// Determines whether the specified header is JPEG.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>True if it is, false otherwise.</returns>
        private static bool IsJpeg(byte[] header)
        {
            return header[0] == 0xFF && header[1] == 0xD8;
        }
    }
}