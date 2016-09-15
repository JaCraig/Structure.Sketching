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
    /// Bits buffer
    /// </summary>
    public class BitsBuffer
    {
        /// <summary>
        /// Gets or sets the accumulator.
        /// </summary>
        /// <value>
        /// The accumulator.
        /// </value>
        public uint Accumulator { get; set; }

        /// <summary>
        /// Gets or sets the mask.
        /// </summary>
        /// <value>
        /// The mask.
        /// </value>
        public uint Mask { get; set; }

        /// <summary>
        /// Gets or sets the number of unread bits.
        /// </summary>
        /// <value>
        /// The number of unread bits.
        /// </value>
        public int NumberOfUnreadBits { get; set; }
    }
}