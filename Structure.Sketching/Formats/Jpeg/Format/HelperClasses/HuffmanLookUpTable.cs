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
    /// Huffman look up table
    /// </summary>
    public class HuffmanLookUpTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HuffmanLookUpTable"/> class.
        /// </summary>
        /// <param name="spec">The spec.</param>
        public HuffmanLookUpTable(HuffmanSpec spec)
        {
            int maxValue = 0;

            foreach (var v in spec.Values)
            {
                if (v > maxValue)
                    maxValue = v;
            }

            Values = new uint[maxValue + 1];

            int code = 0;
            int k = 0;

            for (int i = 0; i < spec.Count.Length; i++)
            {
                int nBits = (i + 1) << 24;
                for (int j = 0; j < spec.Count[i]; j++)
                {
                    Values[spec.Values[k]] = (uint)(nBits | code);
                    code++;
                    k++;
                }
                code <<= 1;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public uint[] Values { get; private set; }
    }
}