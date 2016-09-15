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

using System;
using System.IO;
using System.IO.Compression;

namespace Structure.Sketching.Formats.Png.Format.Helpers.ZLib
{
    /// <summary>
    /// Inflate stream
    /// </summary>
    /// <seealso cref="System.IO.Stream" />
    public class InflateStream : Stream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InflateStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.ArgumentException">Bad compression method</exception>
        public InflateStream(Stream stream)
        {
            InternalStream = stream;
            int CMF = InternalStream.ReadByte();
            int Flag = InternalStream.ReadByte();
            if (CMF == -1 || Flag == -1)
                return;
            if ((CMF & 0x0f) != 8)
            {
                throw new ArgumentException("Bad compression method");
            }
            if ((Flag & 32) != 0)
            {
                byte[] DictionaryID = new byte[4];

                for (int x = 0; x < 4; ++x)
                {
                    DictionaryID[x] = (byte)InternalStream.ReadByte();
                }
            }
            InternalDeflateStream = new DeflateStream(InternalStream, CompressionMode.Decompress, true);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek => false;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <exception cref="System.NotSupportedException">
        /// </exception>
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// The read crc data.
        /// </summary>
        private byte[] CRCData { get; set; }

        /// <summary>
        /// Gets or sets the deflate stream.
        /// </summary>
        /// <value>
        /// The deflate stream.
        /// </value>
        private DeflateStream InternalDeflateStream { get; set; }

        /// <summary>
        /// The internal stream
        /// </summary>
        private Stream InternalStream { get; set; }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            InternalDeflateStream?.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int ReadCount = InternalDeflateStream.Read(buffer, offset, count);
            if (ReadCount < 1 && CRCData == null)
            {
                CRCData = new byte[4];
                for (int x = 0; x < 4; ++x)
                {
                    CRCData[x] = (byte)InternalStream.ReadByte();
                }
            }
            return ReadCount;
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (InternalDeflateStream != null)
                {
                    InternalDeflateStream.Dispose();
                    InternalDeflateStream = null;

                    if (CRCData == null)
                    {
                        CRCData = new byte[4];
                        for (int x = 0; x < 4; ++x)
                        {
                            CRCData[x] = (byte)InternalStream.ReadByte();
                        }
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}