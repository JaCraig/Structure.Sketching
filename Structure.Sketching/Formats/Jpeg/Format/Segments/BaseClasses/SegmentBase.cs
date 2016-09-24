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

using Structure.Sketching.Exceptions;
using Structure.Sketching.Formats.Jpeg.Format.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Sketching.Formats.Jpeg.Format.Segments
{
    /// <summary>
    /// Abstract Class that defines an individual segment of the jpeg.
    /// </summary>
    public abstract class SegmentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentBase" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="bytes">The bytes.</param>
        protected SegmentBase(SegmentTypes type, ByteBuffer bytes)
        {
            Bytes = bytes;
            Type = type;
            TempData = new byte[2 * Block.BlockSize];
        }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public ByteBuffer Bytes { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; protected set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public SegmentTypes Type { get; private set; }

        /// <summary>
        /// Gets or sets the temporary data.
        /// </summary>
        /// <value>
        /// The temporary data.
        /// </value>
        protected byte[] TempData { get; set; }

        /// <summary>
        /// Gets the segment actions.
        /// </summary>
        /// <value>
        /// The segment actions.
        /// </value>
        private static IDictionary<SegmentTypes, Func<ByteBuffer, SegmentBase>> SegmentActions => new Dictionary<SegmentTypes, Func<ByteBuffer, SegmentBase>>
        {
            [SegmentTypes.StartOfImage] = x => new StartOfImage(x),
            [SegmentTypes.EndOfImage] = x => new EndOfImage(x),
            [SegmentTypes.StartOfFrame0] = x => new StartOfFrame(SegmentTypes.StartOfFrame0, x),
            [SegmentTypes.StartOfFrame1] = x => new StartOfFrame(SegmentTypes.StartOfFrame1, x),
            [SegmentTypes.StartOfFrame2] = x => new StartOfFrame(SegmentTypes.StartOfFrame2, x),
            [SegmentTypes.DefineHuffmanTable] = x => new DefineHuffmanTable(x),
            [SegmentTypes.DefineRestartInterval] = x => new DefineRestartInterval(x),
            [SegmentTypes.DefineQuantizationTable] = x => new DefineQuantizationTable(x),
            [SegmentTypes.StartOfScan] = x => new StartOfScan(x)
        };

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="segmentsSeen">The segments seen.</param>
        /// <returns>The segment</returns>
        public static SegmentBase Read(ByteBuffer stream, List<SegmentBase> segmentsSeen)
        {
            var SegmentMarker = GetSegmentMarker(stream);
            if (!SegmentActions.ContainsKey((SegmentTypes)SegmentMarker))
            {
                var Length = GetLength(stream);
                stream.Ignore(Length);
                return null;
            }
            var TempSegment = segmentsSeen.FirstOrDefault(x => x.Type == (SegmentTypes)SegmentMarker);
            if (TempSegment != null)
                return TempSegment;
            return SegmentActions[(SegmentTypes)SegmentMarker](stream);
        }

        /// <summary>
        /// Setups the specified segments.
        /// </summary>
        /// <param name="segments">The segments.</param>
        public abstract void Setup(IEnumerable<SegmentBase> segments);

        /// <summary>
        /// Writes the information to the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        public abstract void Write(BinaryWriter writer);

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="ImageException">short segment length</exception>
        protected static int GetLength(ByteBuffer stream)
        {
            var TempLength = new byte[2];
            stream.ReadFull(TempLength, 0, 2);
            int SegmentLength = ((int)TempLength[0] << 8) + (int)TempLength[1] - 2;
            if (SegmentLength < 0)
                throw new ImageException("short segment length");
            return SegmentLength;
        }

        /// <summary>
        /// Writes the segment header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected void WriteSegmentHeader(BinaryWriter writer)
        {
            writer.Write((byte)0xFF);
            writer.Write(Type);
            writer.Write((byte)(Length >> 8));
            writer.Write((byte)(Length & 0xff));
        }

        private static int GetSegmentMarker(ByteBuffer stream)
        {
            byte[] TempBuffer = new byte[2];
            byte SegmentMarker;
            while (true)
            {
                stream.ReadFull(TempBuffer, 0, 2);
                while (TempBuffer[0] != 0xff)
                {
                    TempBuffer[0] = TempBuffer[1];
                    TempBuffer[1] = stream.ReadByte();
                }
                SegmentMarker = TempBuffer[1];
                if (SegmentMarker == 0)
                {
                    continue;
                }

                while (SegmentMarker == 0xff)
                {
                    SegmentMarker = stream.ReadByte();
                }
                if (SegmentMarker == SegmentTypes.EndOfImage)
                {
                    break;
                }

                if (SegmentTypes.Restart0 <= SegmentMarker && SegmentMarker <= SegmentTypes.Restart7)
                {
                    continue;
                }
                return SegmentMarker;
            }
            return SegmentMarker;
        }
    }
}