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

using System.IO;

namespace Structure.Sketching.ExtensionMethods
{
    /// <summary>
    /// Stream extensions
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the number of bytes specified from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length of bytes to read.</param>
        /// <returns>The byte array read from the stream</returns>
        public static byte[] ReadBytes(this Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }
    }
}