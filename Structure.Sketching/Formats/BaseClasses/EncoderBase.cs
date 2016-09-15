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

using Structure.Sketching.Formats.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Structure.Sketching.Formats.BaseClasses
{
    /// <summary>
    /// Encoder base class
    /// </summary>
    /// <typeparam name="TFile">The type of the file.</typeparam>
    /// <seealso cref="Structure.Sketching.Formats.Interfaces.IEncoder" />
    public abstract class EncoderBase<TFile> : IEncoder
        where TFile : FileBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncoderBase{TFile}"/> class.
        /// </summary>
        protected EncoderBase()
        {
        }

        /// <summary>
        /// Gets the file extensions.
        /// </summary>
        /// <value>
        /// The file extensions.
        /// </value>
        protected abstract string[] FileExtensions { get; }

        /// <summary>
        /// Determines whether this instance can encode the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// True if it can, false otherwise
        /// </returns>
        public bool CanEncode(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            return FileExtensions.Any(x => fileName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Encodes an animation and places it in the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="animation">The animation to encode.</param>
        /// <returns>
        /// True if it encoded successfully, false otherwise
        /// </returns>
        public bool Encode(BinaryWriter writer, Animation animation)
        {
            if (writer == null || animation == null) return false;
            return new TFile().Write(writer, animation);
        }

        /// <summary>
        /// Encodes an image and places it in the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="image">The image to encode.</param>
        /// <returns>
        /// True if it encoded successfully, false otherwise
        /// </returns>
        public bool Encode(BinaryWriter writer, Image image)
        {
            if (writer == null || image == null) return false;
            return new TFile().Write(writer, image);
        }
    }
}