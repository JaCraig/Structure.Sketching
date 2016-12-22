using Structure.Sketching.ExtensionMethods;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Structure.Sketching.Benchmarks.GenericSpeedTests.TestClasses
{
    /// <summary>
    /// Color struct
    /// </summary>
    /// <seealso cref="IEquatable{Color}"/>
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ColorStruct : IEquatable<ColorStruct>
    {
        [FieldOffset(0)]
        public int IntData;

        [FieldOffset(0)]
        public uint UIntData;

        [FieldOffset(0)]
        public byte Red;

        [FieldOffset(1)]
        public byte Green;

        [FieldOffset(2)]
        public byte Blue;

        [FieldOffset(3)]
        public byte Alpha;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <param name="alpha">The alpha.</param>
        public ColorStruct(byte red, byte green = 0, byte blue = 0, byte alpha = 255)
            : this(new byte[] { red, green, blue, alpha })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <exception cref="System.ArgumentException">Hex value is not convertable</exception>
        public ColorStruct(string hex)
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
            UIntData = 0;
            Red = Convert.ToByte(hex.Substring(2, 2), 16);
            Green = Convert.ToByte(hex.Substring(4, 2), 16);
            Blue = Convert.ToByte(hex.Substring(6, 2), 16);
            Alpha = Convert.ToByte(hex.Substring(0, 2), 16);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public ColorStruct(byte[] vector)
        {
            IntData = 0;
            UIntData = 0;
            Red = vector[0];
            Green = vector[1];
            Blue = vector[2];
            Alpha = vector[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="data">The data.</param>
        public ColorStruct(int data)
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            Alpha = 0;
            UIntData = 0;
            IntData = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="data">The data.</param>
        public ColorStruct(uint data)
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            Alpha = 0;
            IntData = 0;
            UIntData = data;
        }

        /// <summary>
        /// Averages the specified colors.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The average color</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct Average(ColorStruct color1, ColorStruct color2)
        {
            return (color1 + color2) / (byte)2;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="Vector3"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator byte[] (ColorStruct color)
        {
            return new byte[] { color.Red, color.Green, color.Blue, color.Alpha };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="hex">The hexadecimal value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColorStruct(string hex)
        {
            return new ColorStruct(hex);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ColorSpaces.Bgra"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColorStruct(int color)
        {
            return new ColorStruct(color);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ColorSpaces.Bgra"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ColorStruct(uint color)
        {
            return new ColorStruct(color);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ColorSpaces.Bgra"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(ColorStruct color)
        {
            return color.IntData;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="Vector4"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector4(ColorStruct color)
        {
            return new Vector4(color.Red / 255f, color.Green / 255f, color.Blue / 255f, color.Alpha / 255f);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator -(ColorStruct color1, ColorStruct color2)
        {
            return new ColorStruct((byte)(color1.Red - color2.Red), (byte)(color1.Green - color2.Green), (byte)(color1.Blue - color2.Blue), (byte)(color1.Alpha - color2.Alpha));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator -(ColorStruct color1, byte factor)
        {
            return new ColorStruct((byte)(color1.Red - factor), (byte)(color1.Green - factor), (byte)(color1.Blue - factor), (byte)(color1.Alpha - factor));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator -(byte factor, ColorStruct color1)
        {
            return new ColorStruct((byte)(factor - color1.Red), (byte)(factor - color1.Green), (byte)(factor - color1.Blue), (byte)(factor - color1.Alpha));
        }

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator !(ColorStruct color)
        {
            return (byte)255 - color;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ColorStruct color1, ColorStruct color2)
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
        public static ColorStruct operator %(ColorStruct color1, byte factor)
        {
            return new ColorStruct((byte)(color1.Red % factor), (byte)(color1.Green % factor), (byte)(color1.Blue % factor), (byte)(color1.Alpha % factor));
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator %(ColorStruct color1, ColorStruct color2)
        {
            return new ColorStruct((byte)(color1.Red % color2.Red), (byte)(color1.Green % color2.Green), (byte)(color1.Blue % color2.Blue), (byte)(color1.Alpha % color2.Alpha));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator *(ColorStruct color1, ColorStruct color2)
        {
            return new ColorStruct((byte)(color1.Red * color2.Red), (byte)(color1.Green * color2.Green), (byte)(color1.Blue * color2.Blue), (byte)(color1.Alpha * color2.Alpha));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator *(ColorStruct color1, float factor)
        {
            return new ColorStruct((byte)(color1.Red * factor), (byte)(color1.Green * factor), (byte)(color1.Blue * factor), (byte)(color1.Alpha * factor));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <param name="color1">The color1.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator *(float factor, ColorStruct color1)
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
        public static ColorStruct operator *(ColorStruct color1, byte factor)
        {
            return new ColorStruct((byte)(color1.Red * factor), (byte)(color1.Green * factor), (byte)(color1.Blue * factor), (byte)(color1.Alpha * factor));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <param name="color1">The color1.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator *(byte factor, ColorStruct color1)
        {
            return color1 * factor;
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator /(ColorStruct color1, ColorStruct color2)
        {
            return new ColorStruct((byte)(color1.Red / color2.Red), (byte)(color1.Green / color2.Green), (byte)(color1.Blue / color2.Blue), (byte)(color1.Alpha / color2.Alpha));
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator /(ColorStruct color1, byte factor)
        {
            return new ColorStruct((byte)(color1.Red / factor), (byte)(color1.Green / factor), (byte)(color1.Blue / factor), (byte)(color1.Alpha / factor));
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator /(ColorStruct color1, float factor)
        {
            return new ColorStruct((byte)(color1.Red / factor), (byte)(color1.Green / factor), (byte)(color1.Blue / factor), (byte)(color1.Alpha / factor));
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator +(ColorStruct color1, ColorStruct color2)
        {
            return new ColorStruct((byte)(color1.Red + color2.Red), (byte)(color1.Green + color2.Green), (byte)(color1.Blue + color2.Blue), (byte)(color1.Alpha + color2.Alpha));
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator +(ColorStruct color1, byte factor)
        {
            return new ColorStruct((byte)(color1.Red + factor), (byte)(color1.Green + factor), (byte)(color1.Blue + factor), (byte)(color1.Alpha + factor));
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorStruct operator +(byte factor, ColorStruct color1)
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
        public static bool operator ==(ColorStruct color1, ColorStruct color2)
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
        public ColorStruct Clamp(byte min = 0, byte max = 255)
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
        public bool Equals(ColorStruct other)
        {
            return other.Alpha == Alpha
                && other.Red == Red
                && other.Green == Green
                && other.Blue == Blue;
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
            return obj is ColorStruct && Equals((ColorStruct)obj);
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
        public ColorStruct Lerp(ColorStruct color, float amount)
        {
            return new ColorStruct(Red.Lerp(color.Red, amount),
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