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

using Structure.Sketching.Formats.Png.Format.Helpers;

namespace Structure.Sketching.Formats.Png.Format
{
    /// <summary>
    /// The end of file indicator
    /// </summary>
    public class FileEnd
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="FileEnd"/> to <see cref="Chunk"/>.
        /// </summary>
        /// <param name="fileEnd">The file end.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Chunk(FileEnd fileEnd)
        {
            return new Chunk(0, ChunkTypes.End, new byte[0]);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Chunk"/> to <see cref="FileEnd"/>.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FileEnd(Chunk chunk)
        {
            return new FileEnd();
        }
    }
}