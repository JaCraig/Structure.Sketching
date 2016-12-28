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
using System.Runtime.InteropServices;

namespace Structure.Sketching.Colors
{
    /// <summary>
    /// Color struct
    /// </summary>
    /// <seealso cref="IEquatable{Color}"/>
    [StructLayout(LayoutKind.Explicit)]
    public partial struct Color : IEquatable<Color>
    {
        /// <summary>
        /// The int data
        /// </summary>
        [FieldOffset(0)]
        public uint IntData;

        /// <summary>
        /// The red
        /// </summary>
        [FieldOffset(0)]
        public byte Red;

        /// <summary>
        /// The green
        /// </summary>
        [FieldOffset(1)]
        public byte Green;

        /// <summary>
        /// The blue
        /// </summary>
        [FieldOffset(2)]
        public byte Blue;

        /// <summary>
        /// The alpha
        /// </summary>
        [FieldOffset(3)]
        public byte Alpha;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <param name="alpha">The alpha.</param>
        public Color(byte red, byte green = 0, byte blue = 0, byte alpha = 255)
            : this(new byte[] { red, green, blue, alpha })
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
            IntData = 0;
            Red = Convert.ToByte(hex.Substring(2, 2), 16);
            Green = Convert.ToByte(hex.Substring(4, 2), 16);
            Blue = Convert.ToByte(hex.Substring(6, 2), 16);
            Alpha = Convert.ToByte(hex.Substring(0, 2), 16);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public Color(byte[] vector)
        {
            IntData = 0;
            Red = vector[0];
            Green = vector[1];
            Blue = vector[2];
            Alpha = vector[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="data">The data.</param>
        public Color(uint data)
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            Alpha = 0;
            IntData = data;
        }

        /// <summary>
        /// Averages the specified colors.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The average color</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Average(Color color1, Color color2)
        {
            return (color1 + color2) / (byte)2;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="Vector3"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator byte[] (Color color)
        {
            return new byte[] { color.Red, color.Green, color.Blue, color.Alpha };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="hex">The hexadecimal value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(string hex)
        {
            return new Color(hex);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ColorSpaces.Bgra"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(uint color)
        {
            return new Color(color);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ColorSpaces.Bgra"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator uint(Color color)
        {
            return color.IntData;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="Vector4"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector4(Color color)
        {
            return new Vector4(color.Red / 255f, color.Green / 255f, color.Blue / 255f, color.Alpha / 255f);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Vector4"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(Vector4 color)
        {
            color = Vector4.Clamp(color, Vector4.Zero, Vector4.One);
            return new Color((byte)(color.X * 255f),
                (byte)(color.Y * 255f),
                (byte)(color.Z * 255f),
                (byte)(color.W * 255f));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(Color color1, Color color2)
        {
            return new Color((byte)(color1.Red - color2.Red).Clamp(0, 255),
                (byte)(color1.Green - color2.Green).Clamp(0, 255),
                (byte)(color1.Blue - color2.Blue).Clamp(0, 255),
                (byte)(color1.Alpha - color2.Alpha).Clamp(0, 255));
        }

        /// <summary>
        /// Subtracts the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Subtract(Color color)
        {
            Red = (byte)(Red - color.Red).Clamp(0, 255);
            Green = (byte)(Green - color.Green).Clamp(0, 255);
            Blue = (byte)(Blue - color.Blue).Clamp(0, 255);
            Alpha = (byte)(Alpha - color.Alpha).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Subtracts the specified factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Subtract(byte factor)
        {
            Red = (byte)(Red - factor).Clamp(0, 255);
            Green = (byte)(Green - factor).Clamp(0, 255);
            Blue = (byte)(Blue - factor).Clamp(0, 255);
            Alpha = (byte)(Alpha - factor).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Adds the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Add(Color color)
        {
            Red = (byte)(Red + color.Red).Clamp(0, 255);
            Green = (byte)(Green + color.Green).Clamp(0, 255);
            Blue = (byte)(Blue + color.Blue).Clamp(0, 255);
            Alpha = (byte)(Alpha + color.Alpha).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Adds the specified factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Add(byte factor)
        {
            Red = (byte)(Red + factor).Clamp(0, 255);
            Green = (byte)(Green + factor).Clamp(0, 255);
            Blue = (byte)(Blue + factor).Clamp(0, 255);
            Alpha = (byte)(Alpha + factor).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Nots this instance.
        /// </summary>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Not()
        {
            IntData = uint.MaxValue - IntData;
            return this;
        }

        /// <summary>
        /// Moduloes the specified factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Modulo(byte factor)
        {
            if (factor == 0)
                return this;
            var ScaledFactor = (factor / 255f);
            Red = (byte)(((Red / 255f) % ScaledFactor) * 255f);
            Green = (byte)(((Green / 255f) % ScaledFactor) * 255f);
            Blue = (byte)(((Blue / 255f) % ScaledFactor) * 255f);
            Alpha = (byte)(((Alpha / 255f) % ScaledFactor) * 255f);
            return this;
        }

        /// <summary>
        /// Moduloes the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Modulo(Color color)
        {
            Red = color.Red == 0 ? Red : (byte)(((Red / 255f) % (color.Red / 255f)) * 255f);
            Green = color.Green == 0 ? Green : (byte)(((Green / 255f) % (color.Green / 255f)) * 255f);
            Blue = color.Blue == 0 ? Blue : (byte)(((Blue / 255f) % (color.Blue / 255f)) * 255f);
            Alpha = color.Alpha == 0 ? Alpha : (byte)(((Alpha / 255f) % (color.Alpha / 255f)) * 255f);
            return this;
        }

        /// <summary>
        /// Multiplies the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Multiply(Color color)
        {
            Red = (byte)(((Red / 255f) * (color.Red / 255f)) * 255f).Clamp(0, 255);
            Green = (byte)(((Green / 255f) * (color.Green / 255f)) * 255f).Clamp(0, 255);
            Blue = (byte)(((Blue / 255f) * (color.Blue / 255f)) * 255f).Clamp(0, 255);
            Alpha = (byte)(((Alpha / 255f) * (color.Alpha / 255f)) * 255f).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Multiplies the specified factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Multiply(float factor)
        {
            Red = (byte)(Red * factor).Clamp(0, 255);
            Green = (byte)(Green * factor).Clamp(0, 255);
            Blue = (byte)(Blue * factor).Clamp(0, 255);
            Alpha = (byte)(Alpha * factor).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Multiplies the specified factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Multiply(byte factor)
        {
            var ScaledFactor = factor / 255f;
            Red = (byte)(((Red / 255f) * ScaledFactor) * 255f).Clamp(0, 255);
            Green = (byte)(((Green / 255f) * ScaledFactor) * 255f).Clamp(0, 255);
            Blue = (byte)(((Blue / 255f) * ScaledFactor) * 255f).Clamp(0, 255);
            Alpha = (byte)(((Alpha / 255f) * ScaledFactor) * 255f).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Ands the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color And(Color color)
        {
            IntData = IntData & color.IntData;
            return this;
        }

        /// <summary>
        /// Ors the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Or(Color color)
        {
            IntData = IntData | color.IntData;
            return this;
        }

        /// <summary>
        /// XOrs the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color XOr(Color color)
        {
            IntData = IntData ^ color.IntData;
            return this;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(Color color1, byte factor)
        {
            return new Color((byte)(color1.Red - factor).Clamp(0, 255),
                (byte)(color1.Green - factor).Clamp(0, 255),
                (byte)(color1.Blue - factor).Clamp(0, 255),
                (byte)(color1.Alpha - factor).Clamp(0, 255));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(byte factor, Color color1)
        {
            return new Color((byte)(factor - color1.Red).Clamp(0, 255),
                (byte)(factor - color1.Green).Clamp(0, 255),
                (byte)(factor - color1.Blue).Clamp(0, 255),
                (byte)(factor - color1.Alpha).Clamp(0, 255));
        }

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator !(Color color)
        {
            return new Color(uint.MaxValue - color.IntData);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
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
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator %(Color color1, byte factor)
        {
            if (factor == 0)
                return new Color(color1.Red, color1.Green, color1.Blue, color1.Alpha);
            var ScaledFactor = (factor / 255f);
            return new Color((byte)(((color1.Red / 255f) % ScaledFactor) * 255f),
                (byte)(((color1.Green / 255f) % ScaledFactor) * 255f),
                (byte)(((color1.Blue / 255f) % ScaledFactor) * 255f),
                (byte)(((color1.Alpha / 255f) % ScaledFactor) * 255f));
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator %(Color color1, Color color2)
        {
            return new Color(color2.Red == 0 ? color1.Red : (byte)(((color1.Red / 255f) % (color2.Red / 255f)) * 255f),
                color2.Green == 0 ? color1.Green : (byte)(((color1.Green / 255f) % (color2.Green / 255f)) * 255f),
                color2.Blue == 0 ? color1.Blue : (byte)(((color1.Blue / 255f) % (color2.Blue / 255f)) * 255f),
                color2.Alpha == 0 ? color1.Alpha : (byte)(((color1.Alpha / 255f) % (color2.Alpha / 255f)) * 255f));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color color1, Color color2)
        {
            return new Color((byte)(((color1.Red / 255f) * (color2.Red / 255f)) * 255f).Clamp(0, 255),
                (byte)(((color1.Green / 255f) * (color2.Green / 255f)) * 255f).Clamp(0, 255),
                (byte)(((color1.Blue / 255f) * (color2.Blue / 255f)) * 255f).Clamp(0, 255),
                (byte)(((color1.Alpha / 255f) * (color2.Alpha / 255f)) * 255f).Clamp(0, 255));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color color1, float factor)
        {
            return new Color((byte)(color1.Red * factor).Clamp(0, 255),
                (byte)(color1.Green * factor).Clamp(0, 255),
                (byte)(color1.Blue * factor).Clamp(0, 255),
                (byte)(color1.Alpha * factor).Clamp(0, 255));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <param name="color1">The color1.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(float factor, Color color1)
        {
            return color1 * factor;
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color color1, byte factor)
        {
            var ScaledFactor = factor / 255f;
            return new Color((byte)(((color1.Red / 255f) * ScaledFactor) * 255f).Clamp(0, 255),
                (byte)(((color1.Green / 255f) * ScaledFactor) * 255f).Clamp(0, 255),
                (byte)(((color1.Blue / 255f) * ScaledFactor) * 255f).Clamp(0, 255),
                (byte)(((color1.Alpha / 255f) * ScaledFactor) * 255f).Clamp(0, 255));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <param name="color1">The color1.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(byte factor, Color color1)
        {
            return color1 * factor;
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator &(Color color1, Color color2)
        {
            return new Color(color1.IntData & color2.IntData);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator |(Color color1, Color color2)
        {
            return new Color(color1.IntData | color2.IntData);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator ^(Color color1, Color color2)
        {
            return new Color(color1.IntData ^ color2.IntData);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color color1, Color color2)
        {
            return new Color(color2.Red == 0 ? color1.Red : (byte)(((color1.Red / 255f) / (color2.Red / 255f)) * 255f),
                color2.Green == 0 ? color1.Green : (byte)(((color1.Green / 255f) / (color2.Green / 255f)) * 255f),
                color2.Blue == 0 ? color1.Blue : (byte)(((color1.Blue / 255f) / (color2.Blue / 255f)) * 255f),
                color2.Alpha == 0 ? color1.Alpha : (byte)(((color1.Alpha / 255f) / (color2.Alpha / 255f)) * 255f));
        }

        /// <summary>
        /// Divides the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Divide(Color color)
        {
            Red = color.Red == 0 ? Red : (byte)((((Red / 255f) / (color.Red / 255f)) * 255f).Clamp(0, 255));
            Green = color.Green == 0 ? Green : (byte)((((Green / 255f) / (color.Green / 255f)) * 255f).Clamp(0, 255));
            Blue = color.Blue == 0 ? Blue : (byte)((((Blue / 255f) / (color.Blue / 255f)) * 255f).Clamp(0, 255));
            Alpha = color.Alpha == 0 ? Alpha : (byte)((((Alpha / 255f) / (color.Alpha / 255f)) * 255f).Clamp(0, 255));
            return this;
        }

        /// <summary>
        /// Divides the specified factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Divide(byte factor)
        {
            if (factor == 0)
                return this;
            var ScaledFactor = (factor / 255f);
            Red = (byte)((((Red / 255f) / ScaledFactor) * 255f).Clamp(0, 255));
            Green = (byte)((((Green / 255f) / ScaledFactor) * 255f).Clamp(0, 255));
            Blue = (byte)((((Blue / 255f) / ScaledFactor) * 255f).Clamp(0, 255));
            Alpha = (byte)((((Alpha / 255f) / ScaledFactor) * 255f).Clamp(0, 255));
            return this;
        }

        /// <summary>
        /// Divides the specified factor.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns>This</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Divide(float factor)
        {
            if (Math.Abs(factor) < EPSILON)
                return this;
            Red = (byte)(Red / factor).Clamp(0, 255);
            Green = (byte)(Green / factor).Clamp(0, 255);
            Blue = (byte)(Blue / factor).Clamp(0, 255);
            Alpha = (byte)(Alpha / factor).Clamp(0, 255);
            return this;
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color color1, byte factor)
        {
            if (factor == 0)
                return new Color(color1.Red, color1.Green, color1.Blue, color1.Alpha);
            var ScaledFactor = (factor / 255f);
            return new Color((byte)((((color1.Red / 255f) / ScaledFactor) * 255f).Clamp(0, 255)),
                (byte)((((color1.Green / 255f) / ScaledFactor) * 255f).Clamp(0, 255)),
                (byte)((((color1.Blue / 255f) / ScaledFactor) * 255f).Clamp(0, 255)),
                (byte)((((color1.Alpha / 255f) / ScaledFactor) * 255f).Clamp(0, 255)));
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color color1, float factor)
        {
            if (Math.Abs(factor) < EPSILON)
                return new Color(color1.Red, color1.Green, color1.Blue, color1.Alpha);
            return new Color((byte)(color1.Red / factor).Clamp(0, 255),
                (byte)(color1.Green / factor).Clamp(0, 255),
                (byte)(color1.Blue / factor).Clamp(0, 255),
                (byte)(color1.Alpha / factor).Clamp(0, 255));
        }

        private static float EPSILON = 0.01f;

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator +(Color color1, Color color2)
        {
            return new Color((byte)(color1.Red + color2.Red).Clamp(0, 255),
                (byte)(color1.Green + color2.Green).Clamp(0, 255),
                (byte)(color1.Blue + color2.Blue).Clamp(0, 255),
                (byte)(color1.Alpha + color2.Alpha).Clamp(0, 255));
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator +(Color color1, byte factor)
        {
            return new Color((byte)(color1.Red + factor).Clamp(0, 255),
                (byte)(color1.Green + factor).Clamp(0, 255),
                (byte)(color1.Blue + factor).Clamp(0, 255),
                (byte)(color1.Alpha + factor).Clamp(0, 255));
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator +(byte factor, Color color1)
        {
            return color1 + factor;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
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
        public Color Clamp(byte min = 0, byte max = 255)
        {
            Red = Red.Clamp(min, max);
            Green = Green.Clamp(min, max);
            Blue = Blue.Clamp(min, max);
            Alpha = Alpha.Clamp(min, max);
            return this;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Color other)
        {
            return other.IntData == IntData;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
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
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var hash = Red.GetHashCode();
            hash = ComputeHash(hash, Green);
            hash = ComputeHash(hash, Blue);
            return ComputeHash(hash, Alpha);
        }

        /// <summary>
        /// Linearly interpolates a value to the destination based on the amount specified
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>The resulting value</returns>
        public Color Lerp(Color color, float amount)
        {
            return new Color(Red.Lerp(color.Red, amount),
                Green.Lerp(color.Green, amount),
                Blue.Lerp(color.Blue, amount),
                Alpha.Lerp(color.Alpha, amount));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({Red},{Green},{Blue},{Alpha})";

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="hash">The existing hash.</param>
        /// <param name="component">The component.</param>
        /// <returns>The resulting hash</returns>
        private int ComputeHash(int hash, byte component)
        {
            return ((hash << 5) + hash) ^ component.GetHashCode();
        }
    }
}