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
    /// Endian based binary writer
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class EndianBinaryWriter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndianBinaryWriter"/> class.
        /// </summary>
        /// <param name="bitConverter">The bit converter.</param>
        /// <param name="stream">The stream.</param>
        public EndianBinaryWriter(EndianBitConverterBase bitConverter, Stream stream)
            : this(bitConverter, stream, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndianBinaryWriter"/> class.
        /// </summary>
        /// <param name="bitConverter">The bit converter.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <exception cref="System.ArgumentException">Stream is not writable</exception>
        public EndianBinaryWriter(EndianBitConverterBase bitConverter, Stream stream, Encoding encoding)
        {
            bitConverter = bitConverter ?? new BigEndianBitConverter();
            stream = stream ?? new MemoryStream();
            encoding = encoding ?? Encoding.UTF8;
            if (!stream.CanWrite)
                throw new ArgumentException("Stream is not writable", nameof(stream));
            BaseStream = stream;
            BitConverter = bitConverter;
            Encoding = encoding;
        }

        /// <summary>
        /// Gets the underlying stream of the EndianBinaryWriter.
        /// </summary>
        /// <value>
        /// The base stream.
        /// </value>
        public Stream BaseStream { get; private set; }

        /// <summary>
        /// Gets the bit converter used to write values to the stream
        /// </summary>
        /// <value>
        /// The bit converter.
        /// </value>
        public EndianBitConverterBase BitConverter { get; }

        /// <summary>
        /// Gets the encoding used to write strings
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public Encoding Encoding { get; }

        /// <summary>
        /// Buffer used for temporary storage during conversion from primitives
        /// </summary>
        private readonly byte[] buffer = new byte[16];

        /// <summary>
        /// Buffer used for Write(char)
        /// </summary>
        private readonly char[] charBuffer = new char[1];

        /// <summary>
        /// Closes the writer, including the underlying stream.
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
                Flush();
                BaseStream.Dispose();
                BaseStream = null;
            }
        }

        /// <summary>
        /// Flushes the underlying stream.
        /// </summary>
        public void Flush()
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is null");
            BaseStream.Flush();
        }

        /// <summary>
        /// Seeks within the stream.
        /// </summary>
        /// <param name="offset">Offset to seek to.</param>
        /// <param name="origin">Origin of seek operation.</param>
        public void Seek(int offset, SeekOrigin origin)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is null");
            BaseStream.Seek(offset, origin);
        }

        /// <summary>
        /// Writes a boolean value to the stream. 1 byte is written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(bool value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 1);
        }

        /// <summary>
        /// Writes a 16-bit signed integer to the stream, using the bit converter
        /// for this writer. 2 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(short value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 2);
        }

        /// <summary>
        /// Writes a 32-bit signed integer to the stream, using the bit converter
        /// for this writer. 4 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(int value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 4);
        }

        /// <summary>
        /// Writes a 64-bit signed integer to the stream, using the bit converter
        /// for this writer. 8 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(long value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 8);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer to the stream, using the bit converter
        /// for this writer. 2 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(ushort value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 2);
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer to the stream, using the bit converter
        /// for this writer. 4 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(uint value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 4);
        }

        /// <summary>
        /// Writes a 64-bit unsigned integer to the stream, using the bit converter
        /// for this writer. 8 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(ulong value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 8);
        }

        /// <summary>
        /// Writes a single-precision floating-point value to the stream, using the bit converter
        /// for this writer. 4 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(float value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 4);
        }

        /// <summary>
        /// Writes a double-precision floating-point value to the stream, using the bit converter
        /// for this writer. 8 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(double value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 8);
        }

        /// <summary>
        /// Writes a decimal value to the stream, using the bit converter for this writer.
        /// 16 bytes are written.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(decimal value)
        {
            BitConverter.CopyBytes(value, buffer, 0);
            WriteInternal(buffer, 16);
        }

        /// <summary>
        /// Writes a signed byte to the stream.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(byte value)
        {
            buffer[0] = value;
            WriteInternal(buffer, 1);
        }

        /// <summary>
        /// Writes an unsigned byte to the stream.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(sbyte value)
        {
            buffer[0] = unchecked((byte)value);
            WriteInternal(buffer, 1);
        }

        /// <summary>
        /// Writes an array of bytes to the stream.
        /// </summary>
        /// <param name="value">The values to write</param>
        public void Write(byte[] value)
        {
            value = value ?? new byte[0];
            WriteInternal(value, value.Length);
        }

        /// <summary>
        /// Writes a portion of an array of bytes to the stream.
        /// </summary>
        /// <param name="value">An array containing the bytes to write</param>
        /// <param name="offset">The index of the first byte to write within the array</param>
        /// <param name="count">The number of bytes to write</param>
        public void Write(byte[] value, int offset, int count)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is null");
            value = value ?? new byte[0];
            BaseStream.Write(value, offset, count);
        }

        /// <summary>
        /// Writes a single character to the stream, using the encoding for this writer.
        /// </summary>
        /// <param name="value">The value to write</param>
        public void Write(char value)
        {
            charBuffer[0] = value;
            Write(charBuffer);
        }

        /// <summary>
        /// Writes an array of characters to the stream, using the encoding for this writer.
        /// </summary>
        /// <param name="value">An array containing the characters to write</param>
        public void Write(char[] value)
        {
            value = value ?? new char[0];
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is null");
            var data = Encoding.GetBytes(value, 0, value.Length);
            WriteInternal(data, data.Length);
        }

        /// <summary>
        /// Writes a string to the stream, using the encoding for this writer.
        /// </summary>
        /// <param name="value">The value to write. Must not be null.</param>
        /// <exception cref="ArgumentNullException">value is null</exception>
        public void Write(string value)
        {
            value = value ?? "";
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is null");
            var data = Encoding.GetBytes(value);
            Write7BitEncodedInt(data.Length);
            WriteInternal(data, data.Length);
        }

        /// <summary>
        /// Writes a 7-bit encoded integer from the stream. This is stored with the least significant
        /// information first, with 7 bits of information per byte of value, and the top
        /// bit as a continuation flag.
        /// </summary>
        /// <param name="value">The 7-bit encoded integer to write to the stream</param>
        public void Write7BitEncodedInt(int value)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is null");
            value = value < 0 ? 0 : value;
            int index = 0;
            while (value >= 128)
            {
                buffer[index++] = (byte)((value & 0x7f) | 0x80);
                value = value >> 7;
                index++;
            }

            buffer[index++] = (byte)value;
            BaseStream.Write(buffer, 0, index);
        }

        /// <summary>
        /// Writes the specified number of bytes from the start of the given byte array,
        /// after checking whether or not the writer has been disposed.
        /// </summary>
        /// <param name="bytes">The array of bytes to write from</param>
        /// <param name="length">The number of bytes to write</param>
        private void WriteInternal(byte[] bytes, int length)
        {
            if (BaseStream == null)
                throw new NullReferenceException("Base stream is null");
            BaseStream.Write(bytes, 0, length);
        }
    }
}