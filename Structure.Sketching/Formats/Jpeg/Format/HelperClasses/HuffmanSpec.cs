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

namespace Structure.Sketching.Formats.Jpeg.Format.HelperClasses
{
    /// <summary>
    /// Huffman spec data holder
    /// </summary>
    public class HuffmanSpec
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HuffmanSpec" /> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="values">The values.</param>
        public HuffmanSpec(byte[] count, byte[] values)
        {
            Count = count;
            Values = values;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public byte[] Count { get; private set; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public byte[] Values { get; private set; }
    }
}