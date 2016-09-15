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
    /// Animation format interface
    /// </summary>
    public interface IAnimationFormat : IFormat
    {
        /// <summary>
        /// Decodes the specified stream and returns an image
        /// </summary>
        /// <param name="stream">The stream containing the image data.</param>
        /// <returns>The resulting image</returns>
        Animation DecodeAnimation(Stream stream);

        /// <summary>
        /// Encodes an image and places it in the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="image">The image to encode.</param>
        /// <returns>True if it is encoded successfully, false otherwise.</returns>
        bool Encode(BinaryWriter writer, Animation image);
    }
}