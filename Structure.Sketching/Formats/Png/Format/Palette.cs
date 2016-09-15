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

using Structure.Sketching.Formats.Png.Format.Enums;
using Structure.Sketching.Formats.Png.Format.Helpers;

namespace Structure.Sketching.Formats.Png.Format
{
    /// <summary>
    /// Palette class
    /// </summary>
    public class Palette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Palette" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        public Palette(byte[] data, PaletteType type)
        {
            Type = type;
            Data = data;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public PaletteType Type { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Structure.Sketching.Formats.Png.Format.Helpers.Chunk" /> to <see cref="Structure.Sketching.Formats.Png.Format.Palette" />.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Palette(Chunk chunk)
        {
            return new Palette(chunk.Data, chunk.Type == ChunkTypes.Palette ? PaletteType.Color : PaletteType.Alpha);
        }
    }
}