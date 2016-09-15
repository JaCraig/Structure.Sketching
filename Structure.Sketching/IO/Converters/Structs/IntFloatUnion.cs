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

using System.Runtime.InteropServices;

namespace Structure.Sketching.IO.Converters.Structs
{
    /// <summary>
    /// Int/float union struct.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct IntFloatUnion
    {
        /// <summary>
        /// The integer value
        /// </summary>
        [FieldOffset(0)]
        public readonly int IntegerValue;

        /// <summary>
        /// The float value
        /// </summary>
        [FieldOffset(0)]
        public readonly float FloatValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntFloatUnion"/> struct.
        /// </summary>
        /// <param name="integerValue">The integer value.</param>
        public IntFloatUnion(int integerValue)
        {
            FloatValue = 0;
            IntegerValue = integerValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntFloatUnion"/> struct.
        /// </summary>
        /// <param name="floatValue">The float value.</param>
        public IntFloatUnion(float floatValue)
        {
            IntegerValue = 0;
            FloatValue = floatValue;
        }
    }
}