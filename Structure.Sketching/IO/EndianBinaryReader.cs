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

using Structure.Sketching.IO.Converters;
using Structure.Sketching.IO.Converters.BaseClasses;
using System;
using System.IO;
using System.Text;

namespace Structure.Sketching.IO
{
    /// <summary>
    /// Endian binary reader
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class EndianBinaryReader : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndianBinaryReader"/> class.
        /// </summary>
        /// <param name="bitConverter">The bit converter.</param>
        /// <param name="stream">The stream.</param>
        public EndianBinaryReader(EndianBitConverterBase bitConverter, Stream stream)
            : this(bitConverter, stream, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndianBinaryReader"/> class.
        /// </summary>
        /// <param name="bitConverter">The bit converter.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <exception cref="System.ArgumentException">Stream is not readable</exception>
        public EndianBinaryReader(EndianBitConverterBase bitConverter, Stream stream, Encoding encoding)
        {
            bitConverter = bitConverter ?? new BigEndianBitConverter();
            stream = stream ?? new MemoryStream();
            encoding = encoding ?? Encoding.UTF8;
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable", nameof(stream));
            BaseStream = stream;
            BitConverter = bitConverter;
            Encoding = encoding;
            decoder = encoding.GetDecoder();
            minBytesPerChar = 1;

            if (encoding is UnicodeEncoding)
            {
                minBytesPerChar = 2;
            }
        }

        /// <summary>
        /// Gets the underlying stream of the EndianBinaryReader.
        /// </summary>
        public Stream BaseStream { get; private set; }

        /// <summary>
        /// Gets the bit converter used to read values from the stream.
        /// </summary>
        public EndianBitConverterBase BitConverter { get; }

        /// <summary>
        /// Gets the encoding used to read strings
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Buffer used for temporary storage before conversion into primitives
        /// </summary>
        private readonly byte[] buffer = new byte[16];

        /// <summary>
        /// Buffer used for temporary storage when reading a single character
        /// </summary>
        private readonly char[] charBuffer = new char[1];

        /// <summary>
        /// Decoder to use for string conversions.
        /// </summary>
        private readonly Decoder decoder;

        /// <summary>
        /// Minimum number of bytes used to encode a character
        /// </summary>
        private readonly int minBytesPerChar;

        /// <summary>
        /// Closes the reader, including the underlying stream.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Disposes of the underlying stream.
        /// </summary>
        public void Dispose()
        {
            if (BaseStream != null)
            {
                BaseStream.Dispose();
                BaseStream = null;
            }
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns>The resulting int.</returns>
        public int Read()
        {
            var charsRead = Read(charBuffer, 0, 1);
            return charsRead == 0 ? -1 : charBuffer[0];
        }

        /// <summary>
        /// Reads the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <returns>The resulting integer</returns>
        /// <exception cref="System.NullReferenceException">Base stream is currently null.</exception>
        public int Read(char[] data, int index, int count)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");
            count = count > data.Length ? data.Length : count;
            data = data ?? new char[0];
            if (index > data.Length - count)
                index = data.Length - count;
            index = index < 0 ? 0 : index;
            count = count < 0 ? 0 : count;

            int read = 0;
            bool firstTime = true;
            byte[] byteBuffer = buffer;

            if (byteBuffer.Length < count * minBytesPerChar)
            {
                byteBuffer = new byte[4096];
            }

            while (read < count)
            {
                int amountToRead;
                if (firstTime)
                {
                    amountToRead = count * minBytesPerChar;
                    firstTime = false;
                }
                else
                {
                    amountToRead = ((count - read - 1) * minBytesPerChar) + 1;
                }

                if (amountToRead > byteBuffer.Length)
                {
                    amountToRead = byteBuffer.Length;
                }

                var bytesRead = TryReadInternal(byteBuffer, amountToRead);
                if (bytesRead == 0)
                {
                    return read;
                }

                var decoded = decoder.GetChars(byteBuffer, 0, bytesRead, data, index);
                read += decoded;
                index += decoded;
            }

            return read;
        }

        /// <summary>
        /// Reads the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <returns>The resulting integer.</returns>
        /// <exception cref="System.NullReferenceException">Base stream is currently null.</exception>
        public int Read(byte[] buffer, int index, int count)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");
            count = count > buffer.Length ? buffer.Length : count;
            buffer = buffer ?? new byte[0];
            if (index > buffer.Length - count)
                index = buffer.Length - count;
            index = index < 0 ? 0 : index;
            count = count < 0 ? 0 : count;

            int read = 0;
            while (count > 0)
            {
                var block = BaseStream.Read(buffer, index, count);
                if (block == 0)
                {
                    return read;
                }

                index += block;
                read += block;
                count -= block;
            }

            return read;
        }

        /// <summary>
        /// Read7s the bit encoded int.
        /// </summary>
        /// <returns>The resulting int.</returns>
        /// <exception cref="System.NullReferenceException">Base stream is currently null.</exception>
        /// <exception cref="System.IO.EndOfStreamException"></exception>
        /// <exception cref="System.IO.IOException">Invalid 7-bit encoded integer in stream.</exception>
        public int Read7BitEncodedInt()
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");

            int ret = 0;
            for (int shift = 0; shift < 35; shift += 7)
            {
                var b = BaseStream.ReadByte();
                if (b == -1)
                {
                    throw new EndOfStreamException();
                }

                ret = ret | ((b & 0x7f) << shift);
                if ((b & 0x80) == 0)
                {
                    return ret;
                }
            }
            throw new IOException("Invalid 7-bit encoded integer in stream.");
        }

        /// <summary>
        /// Reads the big endian7 bit encoded int.
        /// </summary>
        /// <returns>The resulting int.</returns>
        /// <exception cref="System.NullReferenceException">Base stream is currently null.</exception>
        /// <exception cref="System.IO.EndOfStreamException"></exception>
        /// <exception cref="System.IO.IOException">Invalid 7-bit encoded integer in stream.</exception>
        public int ReadBigEndian7BitEncodedInt()
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");

            int ret = 0;
            for (int i = 0; i < 5; i++)
            {
                var b = BaseStream.ReadByte();
                if (b == -1)
                {
                    throw new EndOfStreamException();
                }

                ret = (ret << 7) | (b & 0x7f);
                if ((b & 0x80) == 0)
                {
                    return ret;
                }
            }
            throw new IOException("Invalid 7-bit encoded integer in stream.");
        }

        /// <summary>
        /// Reads the boolean.
        /// </summary>
        /// <returns>The resulting boolean</returns>
        public bool ReadBoolean()
        {
            ReadInternal(buffer, 1);
            return BitConverter.ToBoolean(buffer, 0);
        }

        /// <summary>
        /// Reads a single byte from the stream.
        /// </summary>
        /// <returns>The byte read</returns>
        public byte ReadByte()
        {
            ReadInternal(buffer, 1);
            return buffer[0];
        }

        /// <summary>
        /// Reads the specified number of bytes, returning them in a new byte array.
        /// If not enough bytes are available before the end of the stream, this
        /// method will return what is available.
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns>The bytes read</returns>
        public byte[] ReadBytes(int count)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");
            count = count < 0 ? 0 : count;
            byte[] ret = new byte[count];
            int index = 0;
            while (index < count)
            {
                var read = BaseStream.Read(ret, index, count - index);
                if (read == 0)
                {
                    byte[] copy = new byte[index];
                    Buffer.BlockCopy(ret, 0, copy, 0, index);
                    return copy;
                }

                index += read;
            }

            return ret;
        }

        /// <summary>
        /// Reads the specified number of bytes, returning them in a new byte array.
        /// If not enough bytes are available before the end of the stream, this
        /// method will throw an IOException.
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns>The bytes read</returns>
        public byte[] ReadBytesOrThrow(int count)
        {
            byte[] ret = new byte[count];
            ReadInternal(ret, count);
            return ret;
        }

        /// <summary>
        /// Reads a decimal value from the stream, using the bit converter
        /// for this reader. 16 bytes are read.
        /// </summary>
        /// <returns>The decimal value read</returns>
        public decimal ReadDecimal()
        {
            ReadInternal(buffer, 16);
            return BitConverter.ToDecimal(buffer, 0);
        }

        /// <summary>
        /// Reads a double-precision floating-point value from the stream, using the bit converter
        /// for this reader. 8 bytes are read.
        /// </summary>
        /// <returns>The floating point value read</returns>
        public double ReadDouble()
        {
            ReadInternal(buffer, 8);
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Reads a single-precision floating-point value from the stream, using the bit converter
        /// for this reader. 4 bytes are read.
        /// </summary>
        /// <returns>The floating point value read</returns>
        public float ReadFloat()
        {
            ReadInternal(buffer, 4);
            return BitConverter.ToFloat(buffer, 0);
        }

        /// <summary>
        /// Reads a 32-bit signed integer from the stream, using the bit converter
        /// for this reader. 4 bytes are read.
        /// </summary>
        /// <returns>The 32-bit integer read</returns>
        public int ReadInt()
        {
            ReadInternal(buffer, 4);
            return BitConverter.ToInt(buffer, 0);
        }

        /// <summary>
        /// Reads a 64-bit signed integer from the stream, using the bit converter
        /// for this reader. 8 bytes are read.
        /// </summary>
        /// <returns>The 64-bit integer read</returns>
        public long ReadLong()
        {
            ReadInternal(buffer, 8);
            return BitConverter.ToLong(buffer, 0);
        }

        /// <summary>
        /// Reads a 16-bit signed integer from the stream, using the bit converter
        /// for this reader. 2 bytes are read.
        /// </summary>
        /// <returns>The 16-bit integer read</returns>
        public short ReadShort()
        {
            ReadInternal(buffer, 2);
            return BitConverter.ToShort(buffer, 0);
        }

        /// <summary>
        /// Reads a single signed byte from the stream.
        /// </summary>
        /// <returns>The byte read</returns>
        public sbyte ReadSignedByte()
        {
            ReadInternal(buffer, 1);
            return unchecked((sbyte)buffer[0]);
        }

        /// <summary>
        /// Reads a length-prefixed string from the stream, using the encoding for this reader.
        /// A 7-bit encoded integer is first read, which specifies the number of bytes
        /// to read from the stream. These bytes are then converted into a string with
        /// the encoding for this reader.
        /// </summary>
        /// <returns>The string read from the stream.</returns>
        public string ReadString()
        {
            var bytesToRead = Read7BitEncodedInt();

            byte[] data = new byte[bytesToRead];
            ReadInternal(data, bytesToRead);
            return Encoding.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer from the stream, using the bit converter
        /// for this reader. 4 bytes are read.
        /// </summary>
        /// <returns>The 32-bit unsigned integer read</returns>
        public uint ReadUnsignedInt()
        {
            ReadInternal(buffer, 4);
            return BitConverter.ToUnsignedInteger(buffer, 0);
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer from the stream, using the bit converter
        /// for this reader. 8 bytes are read.
        /// </summary>
        /// <returns>The 64-bit unsigned integer read</returns>
        public ulong ReadUnsignedLong()
        {
            ReadInternal(buffer, 8);
            return BitConverter.ToUnsignedLong(buffer, 0);
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer from the stream, using the bit converter
        /// for this reader. 2 bytes are read.
        /// </summary>
        /// <returns>The 16-bit unsigned integer read</returns>
        public ushort ReadUnsignedShort()
        {
            ReadInternal(buffer, 2);
            return BitConverter.ToUnsignedShort(buffer, 0);
        }

        /// <summary>
        /// Seeks within the stream.
        /// </summary>
        /// <param name="offset">Offset to seek to.</param>
        /// <param name="origin">Origin of seek operation.</param>
        public void Seek(int offset, SeekOrigin origin)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");
            BaseStream.Seek(offset, origin);
        }

        /// <summary>
        /// Reads the given number of bytes from the stream, throwing an exception
        /// if they can't all be read.
        /// </summary>
        /// <param name="data">Buffer to read into</param>
        /// <param name="size">Number of bytes to read</param>
        private void ReadInternal(byte[] data, int size)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");
            int index = 0;
            while (index < size)
            {
                var read = BaseStream.Read(data, index, size - index);
                if (read == 0)
                {
                    throw new EndOfStreamException
                        (
                        string.Format(
                            "End of stream reached with {0} byte{1} left to read.",
                            size - index,
                            size - index == 1 ? "s" : string.Empty));
                }

                index += read;
            }
        }

        /// <summary>
        /// Reads the given number of bytes from the stream if possible, returning
        /// the number of bytes actually read, which may be less than requested if
        /// (and only if) the end of the stream is reached.
        /// </summary>
        /// <param name="data">Buffer to read into</param>
        /// <param name="size">Number of bytes to read</param>
        /// <returns>Number of bytes actually read</returns>
        private int TryReadInternal(byte[] data, int size)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is currently null.");
            int index = 0;
            while (index < size)
            {
                var read = BaseStream.Read(data, index, size - index);
                if (read == 0)
                {
                    return index;
                }

                index += read;
            }

            return index;
        }
    }
}