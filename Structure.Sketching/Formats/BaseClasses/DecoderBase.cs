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
    /// Decoder base class
    /// </summary>
    /// <typeparam name="TFile">The type of the file format class.</typeparam>
    /// <seealso cref="Structure.Sketching.Formats.Interfaces.IDecoder"/>
    public abstract class DecoderBase<TFile> : IDecoder
        where TFile : FileBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecoderBase{TFile}"/> class.
        /// </summary>
        protected DecoderBase()
        {
        }

        /// <summary>
        /// Gets the size of the header.
        /// </summary>
        /// <value>The size of the header.</value>
        public abstract int HeaderSize { get; }

        /// <summary>
        /// Gets the file extensions.
        /// </summary>
        /// <value>The file extensions.</value>
        protected abstract string[] FileExtensions { get; }

        /// <summary>
        /// Determines whether this instance can decode the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanDecode(Stream stream)
        {
            if (stream == null) return false;
            byte[] TempBuffer = new byte[HeaderSize];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(TempBuffer, 0, HeaderSize);
            bool value = CanDecode(TempBuffer);
            stream.Seek(0, SeekOrigin.Begin);
            return value;
        }

        /// <summary>
        /// Determines whether this instance can decode the specified header.
        /// </summary>
        /// <param name="header">The header data</param>
        /// <returns>True if it can, false otherwise</returns>
        public abstract bool CanDecode(byte[] header);

        /// <summary>
        /// Determines whether this instance can decode the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanDecode(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            return FileExtensions.Any(x => fileName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Decodes the specified stream and returns an image
        /// </summary>
        /// <param name="stream">The stream containing the image data.</param>
        /// <returns>The resulting image</returns>
        public Image Decode(Stream stream)
        {
            if (stream == null)
                return new Image(1, 1, new byte[4]);
            return new TFile().Decode(stream);
        }

        /// <summary>
        /// Decodes the specified stream and returns an animation
        /// </summary>
        /// <param name="stream">The stream containing the animation data.</param>
        /// <returns>The resulting animation</returns>
        public Animation DecodeAnimation(Stream stream)
        {
            if (stream == null)
                return new Animation(new Image[] { new Image(1, 1, new byte[4]) }, 0);
            return new TFile().Decode(stream);
        }
    }
}