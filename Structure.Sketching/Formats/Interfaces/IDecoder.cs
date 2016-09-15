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

using System.IO;

namespace Structure.Sketching.Formats.Interfaces
{
    /// <summary>
    /// Decoder interface
    /// </summary>
    public interface IDecoder
    {
        /// <summary>
        /// Gets the size of the header.
        /// </summary>
        /// <value>The size of the header.</value>
        int HeaderSize { get; }

        /// <summary>
        /// Determines whether this instance can decode the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True if it can, false otherwise</returns>
        bool CanDecode(string fileName);

        /// <summary>
        /// Determines whether this instance can decode the specified header.
        /// </summary>
        /// <param name="header">The header data</param>
        /// <returns>True if it can, false otherwise</returns>
        bool CanDecode(byte[] header);

        /// <summary>
        /// Determines whether this instance can decode the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>True if it can, false otherwise</returns>
        bool CanDecode(Stream stream);

        /// <summary>
        /// Decodes the specified stream and returns an image
        /// </summary>
        /// <param name="stream">The stream containing the image data.</param>
        /// <returns>The resulting image</returns>
        Image Decode(Stream stream);

        /// <summary>
        /// Decodes the specified stream and returns an animation
        /// </summary>
        /// <param name="stream">The stream containing the animation data.</param>
        /// <returns>The resulting animation</returns>
        Animation DecodeAnimation(Stream stream);
    }
}