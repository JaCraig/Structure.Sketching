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

using Structure.Sketching.Formats.Jpeg.Format.HelperClasses;
using System.Collections.Generic;
using System.IO;

namespace Structure.Sketching.Formats.Jpeg.Format.Segments
{
    /// <summary>
    /// Application 0 segment
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Jpeg.Format.Segments.SegmentBase" />
    public class Application0 : SegmentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Application0"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public Application0(ByteBuffer bytes)
            : base(SegmentTypes.Application0, bytes)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is jfif.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is jfif; otherwise, <c>false</c>.
        /// </value>
        public bool IsJFIF { get; set; }

        /// <summary>
        /// Setups the specified segments.
        /// </summary>
        /// <param name="segments">The segments.</param>
        public override void Setup(IEnumerable<SegmentBase> segments)
        {
            Length = GetLength(Bytes);
            var n = Length;
            if (n < 5)
            {
                Bytes.Ignore(n);
                return;
            }

            Bytes.ReadFull(TempData, 0, 5);
            n -= 5;

            IsJFIF = TempData[0] == 'J' && TempData[1] == 'F' && TempData[2] == 'I' && TempData[3] == 'F' && TempData[4] == '\x00';
            if (n > 0)
                Bytes.Ignore(n);
        }

        /// <summary>
        /// Writes the information to the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)0xFF);
            writer.Write(Type);
        }
    }
}