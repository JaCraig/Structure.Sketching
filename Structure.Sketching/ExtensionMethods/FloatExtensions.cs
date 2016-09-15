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

using System.Collections.Generic;
using System.Numerics;

namespace Structure.Sketching.ExtensionMethods
{
    /// <summary>
    /// Float extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Clamps the value based on the minimum and maximum specified.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <param name="Min">The minimum.</param>
        /// <param name="Max">The maximum.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        public static float Clamp(this float Value, float Min, float Max)
        {
            return Value < Min ? Min : Value > Max ? Max : Value;
        }

        /// <summary>
        /// Clamps the value based on the minimum and maximum specified.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <param name="Min">The minimum.</param>
        /// <param name="Max">The maximum.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(this int Value, int Min, int Max)
        {
            return Value < Min ? Min : Value > Max ? Max : Value;
        }

        /// <summary>
        /// Converts a float array to a vector4 array.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The resulting array after converting</returns>
        public static Vector4[] ToVector4(this float[] values)
        {
            if (values == null || values.Length == 0)
            {
                return new Vector4[0];
            }
            var NewData = new List<Vector4>();
            for (int x = 0; x < values.Length; x += 4)
            {
                NewData.Add(new Vector4(values[x], values[x + 1], values[x + 2], values[x + 3]));
            }
            return NewData.ToArray();
        }
    }
}