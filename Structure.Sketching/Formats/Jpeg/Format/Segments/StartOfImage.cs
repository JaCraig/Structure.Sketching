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
    /// Start of image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Jpeg.Format.Segments.SegmentBase" />
    public class StartOfImage : SegmentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartOfImage"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public StartOfImage(ByteBuffer buffer)
            : base(SegmentTypes.StartOfImage, buffer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartOfImage"/> class.
        /// </summary>
        public StartOfImage()
            : base(SegmentTypes.StartOfImage, null)
        {
        }

        /// <summary>
        /// Setups the specified segments.
        /// </summary>
        /// <param name="segments">The segments.</param>
        public override void Setup(IEnumerable<SegmentBase> segments)
        {
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