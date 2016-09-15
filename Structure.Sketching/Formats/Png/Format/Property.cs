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
using System.Text;

namespace Structure.Sketching.Formats.Png.Format
{
    /// <summary>
    /// A key, value property
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public Property(string key, string value)
        {
            Value = value ?? string.Empty;
            Key = key ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Chunk"/> to <see cref="Property"/>.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Property(Chunk chunk)
        {
            int Count = 0;
            for (int x = 0; x < chunk.Data.Length; ++x, ++Count)
            {
                if (chunk.Data[x] == 0)
                    break;
            }

            return new Property(Encoding.UTF8.GetString(chunk.Data, 0, Count),
                Encoding.UTF8.GetString(chunk.Data, Count + 1, chunk.Data.Length - Count - 1));
        }
    }
}