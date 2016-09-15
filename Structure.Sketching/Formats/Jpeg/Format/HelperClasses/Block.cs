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
    /// Block class
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Block"/> class.
        /// </summary>
        public Block()
        {
            Data = new int[BlockSize];
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public int[] Data { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="int"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="int"/>.
        /// </value>
        /// <param name="Index">The index.</param>
        /// <returns>The value specified.</returns>
        public int this[int Index]
        {
            get { return Data[Index]; }
            set { Data[Index] = value; }
        }

        /// <summary>
        /// The block size
        /// </summary>
        public const int BlockSize = 64;
    }
}