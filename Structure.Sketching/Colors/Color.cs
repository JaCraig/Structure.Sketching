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

using Structure.Sketching.ExtensionMethods;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Structure.Sketching.Colors
{
    /// <summary>
    /// Color struct
    /// </summary>
    /// <seealso cref="IEquatable{Color}" />
    public partial struct Color : IEquatable<Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <param name="alpha">The alpha.</param>
        public Color(float red, float green = 0, float blue = 0, float alpha = 1)
            : this(new Vector4(red, green, blue, alpha))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <exception cref="System.ArgumentException">Hex value is not convertable</exception>
        public Color(string hex)
        {
            hex = hex.StartsWith("#", StringComparison.Ordinal) ? hex.Substring(1) : hex;
            if (hex.Length == 3)
            {
                hex = "FF"
                    + char.ToString(hex[0])
                    + char.ToString(hex[0])
                    + char.ToString(hex[1])
                    + char.ToString(hex[1])
                    + char.ToString(hex[2])
                    + char.ToString(hex[2]);
            }
            if (hex.Length == 4)
            {
                hex = char.ToString(hex[0])
                    + char.ToString(hex[0])
                    + char.ToString(hex[1])
                    + char.ToString(hex[1])
                    + char.ToString(hex[2])
                    + char.ToString(hex[2])
                    + char.ToString(hex[3])
                    + char.ToString(hex[3]);
            }
            if (hex.Length == 6)
            {
                hex = "FF" + hex;
            }
            if (hex.Length < 8)
            {
                throw new ArgumentException("Hex value is not convertable");
            }
            r = Convert.ToByte(hex.Substring(2, 2), 16) / 255f;
            g = Convert.ToByte(hex.Substring(4, 2), 16) / 255f;
            b = Convert.ToByte(hex.Substring(6, 2), 16) / 255f;
            a = Convert.ToByte(hex.Substring(0, 2), 16) / 255f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public Color(Vector4 vector)
        {
            r = vector.X;
            g = vector.Y;
            b = vector.Z;
            a = vector.W;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public Color(Vector3 vector)
            : this(new Vector4(vector.X, vector.Y, vector.Z, 1))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="alpha">The alpha.</param>
        public Color(Vector3 vector, float alpha)
            : this(new Vector4(vector.X, vector.Y, vector.Z, alpha))
        {
        }

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>
        /// The alpha.
        /// </value>
        public float Alpha
        {
            get { return a; }
            set { a = value; }
        }

        /// <summary>
        /// Gets or sets the blue.
        /// </summary>
        /// <value>
        /// The blue.
        /// </value>
        public float Blue
        {
            get { return b; }
            set { b = value; }
        }

        /// <summary>
        /// Gets or sets the green.
        /// </summary>
        /// <value>
        /// The green.
        /// </value>
        public float Green
        {
            get { return g; }
            set { g = value; }
        }

        /// <summary>
        /// Gets or sets the red.
        /// </summary>
        /// <value>
        /// The red.
        /// </value>
        public float Red
        {
            get { return r; }
            set { r = value; }
        }

        /// <summary>
        /// The epsilon
        /// </summary>
        private const float EPSILON = 0.001f;

        /// <summary>
        /// alpha component
        /// </summary>
        private float a;

        /// <summary>
        /// blue component
        /// </summary>
        private float b;

        /// <summary>
        /// green component
        /// </summary>
        private float g;

        /// <summary>
        /// red component
        /// </summary>
        private float r;

        /// <summary>
        /// Averages the specified colors.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The average color</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Average(Color color1, Color color2)
        {
            return (color1 + color2) * .5f;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Vector4"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Color(Vector4 vector)
        {
            return new Color(vector);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Vector3"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Color(Vector3 vector)
        {
            return new Color(vector);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="hex">The hexadecimal value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Color(string hex)
        {
            return new Color(hex);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="Vector3"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Vector3(Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Structure.Sketching.Colors.Color" /> to <see cref="System.Numerics.Vector4" />.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Vector4(Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(Color color1, Color color2)
        {
            return new Color(color1.r - color2.r, color1.g - color2.g, color1.b - color2.b, color1.a - color2.a);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(Color color1, float factor)
        {
            return new Color(color1.r - factor, color1.g - factor, color1.b - factor, color1.a - factor);
        }

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator !(Color color)
        {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, 1 - color.a);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Color color1, Color color2)
        {
            return !(color1 == color2);
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator %(Color color1, float factor)
        {
            return new Color(color1.r % factor, color1.g % factor, color1.b % factor, color1.a % factor);
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator %(Color color1, Color color2)
        {
            return new Color(color1.r % color2.r, color1.g % color2.g, color1.b % color2.b, color1.a % color2.a);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color color1, Color color2)
        {
            return new Color(color1.r * color2.r, color1.g * color2.g, color1.b * color2.b, color1.a * color2.a);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color color1, float factor)
        {
            return new Color(color1.r * factor, color1.g * factor, color1.b * factor, color1.a * factor);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <param name="color1">The color1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(float factor, Color color1)
        {
            return new Color(color1.r * factor, color1.g * factor, color1.b * factor, color1.a * factor);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color color1, Color color2)
        {
            return new Color(color1.r / color2.r, color1.g / color2.g, color1.b / color2.b, color1.a / color2.a);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color color1, float factor)
        {
            return new Color(color1.r / factor, color1.g / factor, color1.b / factor, color1.a / factor);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator +(Color color1, Color color2)
        {
            return new Color(color1.r + color2.r, color1.g + color2.g, color1.b + color2.b, color1.a + color2.a);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator +(Color color1, float factor)
        {
            return new Color(color1.r + factor, color1.g + factor, color1.b + factor, color1.a + factor);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Color color1, Color color2)
        {
            return color1.Equals(color2);
        }

        /// <summary>
        /// Clamps this instance.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Clamp(float min = 0, float max = 1)
        {
            r = r.Clamp(min, max);
            g = g.Clamp(min, max);
            b = b.Clamp(min, max);
            a = a.Clamp(min, max);
            return this;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Color other)
        {
            return Math.Abs(other.Alpha - a) < EPSILON
                && Math.Abs(other.Red - r) < EPSILON
                && Math.Abs(other.Green - g) < EPSILON
                && Math.Abs(other.Blue - b) < EPSILON;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Color && Equals((Color)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            int hash = r.GetHashCode();
            hash = ComputeHash(hash, g);
            hash = ComputeHash(hash, b);
            return ComputeHash(hash, a);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"({r:#0.##},{g:#0.##},{b:#0.##},{a:#0.##})";

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="hash">The existing hash.</param>
        /// <param name="component">The component.</param>
        /// <returns>The resulting hash</returns>
        private int ComputeHash(int hash, float component)
        {
            return ((hash << 5) + hash) ^ component.GetHashCode();
        }
    }
}