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
    /// Define restart interval
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Jpeg.Format.Segments.SegmentBase" />
    public class DefineRestartInterval : SegmentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefineRestartInterval"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public DefineRestartInterval(ByteBuffer bytes)
            : base(SegmentTypes.DefineRestartInterval, bytes)
        {
        }

        /// <summary>
        /// Gets or sets the restart interval.
        /// </summary>
        /// <value>
        /// The restart interval.
        /// </value>
        public int RestartInterval { get; set; }

        /// <summary>
        /// Setups the specified segments.
        /// </summary>
        /// <param name="segments">The segments.</param>
        public override void Setup(IEnumerable<SegmentBase> segments)
        {
            Length = GetLength(Bytes);
            Bytes.ReadFull(TempData, 0, 2);
            RestartInterval = ((int)TempData[0] << 8) + (int)TempData[1];
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