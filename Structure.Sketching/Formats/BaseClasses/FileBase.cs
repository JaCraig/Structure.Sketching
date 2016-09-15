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
using System.IO;

namespace Structure.Sketching.Formats.BaseClasses
{
    /// <summary>
    /// File base class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Interfaces.IFile" />
    public abstract class FileBase : IFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileBase"/> class.
        /// </summary>
        protected FileBase()
        {
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="FileBase"/> to <see cref="Animation"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Animation(FileBase value)
        {
            if (value == null) return null;
            return value.ToAnimation();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="File"/> to <see cref="Image"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Image(FileBase value)
        {
            if (value == null) return null;
            return value.ToImage();
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>This.</returns>
        public abstract FileBase Decode(Stream stream);

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="image">The image.</param>
        /// <returns>
        /// True if it writes successfully, false otherwise.
        /// </returns>
        public abstract bool Write(BinaryWriter stream, Image image);

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="animation">The animation.</param>
        /// <returns>
        /// True if it writes successfully, false otherwise.
        /// </returns>
        public abstract bool Write(BinaryWriter writer, Animation animation);

        /// <summary>
        /// Converts the file to an animation.
        /// </summary>
        /// <returns>The animation version of the file.</returns>
        protected abstract Animation ToAnimation();

        /// <summary>
        /// Converts the file to an image.
        /// </summary>
        /// <returns>The image version of the file.</returns>
        protected abstract Image ToImage();
    }
}