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
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Structure.Sketching.Numerics
{
    /// <summary>
    /// 5x5 matrix
    /// </summary>
    public struct Matrix5x5 : IEquatable<Matrix5x5>
    {
        /// <summary>
        /// Constructs a Matrix5x5 from the given components.
        /// </summary>
        /// <param name="m11">The M11.</param>
        /// <param name="m12">The M12.</param>
        /// <param name="m13">The M13.</param>
        /// <param name="m14">The M14.</param>
        /// <param name="m15">The M15.</param>
        /// <param name="m21">The M21.</param>
        /// <param name="m22">The M22.</param>
        /// <param name="m23">The M23.</param>
        /// <param name="m24">The M24.</param>
        /// <param name="m25">The M25.</param>
        /// <param name="m31">The M31.</param>
        /// <param name="m32">The M32.</param>
        /// <param name="m33">The M33.</param>
        /// <param name="m34">The M34.</param>
        /// <param name="m35">The M35.</param>
        /// <param name="m41">The M41.</param>
        /// <param name="m42">The M42.</param>
        /// <param name="m43">The M43.</param>
        /// <param name="m44">The M44.</param>
        /// <param name="m45">The M45.</param>
        /// <param name="m51">The M51.</param>
        /// <param name="m52">The M52.</param>
        /// <param name="m53">The M53.</param>
        /// <param name="m54">The M54.</param>
        /// <param name="m55">The M55.</param>
        public Matrix5x5(float m11, float m12, float m13, float m14, float m15,
                         float m21, float m22, float m23, float m24, float m25,
                         float m31, float m32, float m33, float m34, float m35,
                         float m41, float m42, float m43, float m44, float m45,
                         float m51, float m52, float m53, float m54, float m55)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M15 = m15;

            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M25 = m25;

            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M35 = m35;

            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
            M45 = m45;

            M51 = m51;
            M52 = m52;
            M53 = m53;
            M54 = m54;
            M55 = m55;
        }

        /// <summary>
        /// Constructs a Matrix5x5 from the given Matrix4x4.
        /// </summary>
        /// <param name="value">The source Matrix4x4.</param>
        public Matrix5x5(Matrix4x4 value)
        {
            M11 = value.M11;
            M12 = value.M12;
            M13 = value.M13;
            M14 = value.M14;
            M15 = 0f;
            M21 = value.M21;
            M22 = value.M22;
            M23 = value.M23;
            M24 = value.M24;
            M25 = 0f;
            M31 = value.M31;
            M32 = value.M32;
            M33 = value.M33;
            M34 = value.M34;
            M35 = 0f;
            M41 = value.M41;
            M42 = value.M42;
            M43 = value.M43;
            M44 = value.M44;
            M45 = 0f;
            M51 = 0f;
            M52 = 0f;
            M53 = 0f;
            M54 = 0f;
            M55 = 1f;
        }

        /// <summary>
        /// Gets the identity.
        /// </summary>
        /// <value>The identity.</value>
        public static Matrix5x5 Identity => _identity;

        /// <summary>
        /// Returns whether the matrix is the identity matrix.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return M11 == 1f && M22 == 1f && M33 == 1f && M44 == 1f && M55 == 1f &&
                       M12 == 0f && M13 == 0f && M14 == 0f && M15 == 0f &&
                       M21 == 0f && M23 == 0f && M24 == 0f && M25 == 0f &&
                       M31 == 0f && M32 == 0f && M34 == 0f && M35 == 0f &&
                       M41 == 0f && M42 == 0f && M43 == 0f && M45 == 0f &&
                       M51 == 0f && M52 == 0f && M53 == 0f && M54 == 0f;
            }
        }

        /// <summary>
        /// Gets or sets the translation component of this matrix.
        /// </summary>
        public Vector4 Translation
        {
            get
            {
                return new Vector4(M51, M52, M53, M54);
            }
            set
            {
                M51 = value.X;
                M52 = value.Y;
                M53 = value.Z;
                M54 = value.W;
            }
        }

        /// <summary>
        /// Value at row 1, column 1 of the matrix.
        /// </summary>
        public float M11;

        /// <summary>
        /// Value at row 1, column 2 of the matrix.
        /// </summary>
        public float M12;

        /// <summary>
        /// Value at row 1, column 3 of the matrix.
        /// </summary>
        public float M13;

        /// <summary>
        /// Value at row 1, column 4 of the matrix.
        /// </summary>
        public float M14;

        /// <summary>
        /// Value at row 3, column 5 of the matrix.
        /// </summary>
        public float M15;

        /// <summary>
        /// Value at row 2, column 1 of the matrix.
        /// </summary>
        public float M21;

        /// <summary>
        /// Value at row 2, column 2 of the matrix.
        /// </summary>
        public float M22;

        /// <summary>
        /// Value at row 2, column 3 of the matrix.
        /// </summary>
        public float M23;

        /// <summary>
        /// Value at row 2, column 4 of the matrix.
        /// </summary>
        public float M24;

        /// <summary>
        /// Value at row 2, column 5 of the matrix.
        /// </summary>
        public float M25;

        /// <summary>
        /// Value at row 3, column 1 of the matrix.
        /// </summary>
        public float M31;

        /// <summary>
        /// Value at row 3, column 2 of the matrix.
        /// </summary>
        public float M32;

        /// <summary>
        /// Value at row 3, column 3 of the matrix.
        /// </summary>
        public float M33;

        /// <summary>
        /// Value at row 3, column 4 of the matrix.
        /// </summary>
        public float M34;

        /// <summary>
        /// Value at row 3, column 5 of the matrix.
        /// </summary>
        public float M35;

        /// <summary>
        /// Value at row 4, column 1 of the matrix.
        /// </summary>
        public float M41;

        /// <summary>
        /// Value at row 4, column 2 of the matrix.
        /// </summary>
        public float M42;

        /// <summary>
        /// Value at row 4, column 3 of the matrix.
        /// </summary>
        public float M43;

        /// <summary>
        /// Value at row 4, column 4 of the matrix.
        /// </summary>
        public float M44;

        /// <summary>
        /// Value at row 4, column 5 of the matrix.
        /// </summary>
        public float M45;

        /// <summary>
        /// Value at row 5, column 1 of the matrix.
        /// </summary>
        public float M51;

        /// <summary>
        /// Value at row 5, column 2 of the matrix.
        /// </summary>
        public float M52;

        /// <summary>
        /// Value at row 5, column 3 of the matrix.
        /// </summary>
        public float M53;

        /// <summary>
        /// Value at row 5, column 4 of the matrix.
        /// </summary>
        public float M54;

        /// <summary>
        /// Value at row 5, column 5 of the matrix.
        /// </summary>
        public float M55;

        private static readonly Matrix5x5 _identity = new Matrix5x5
        (
            1f, 0f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f, 0f,
            0f, 0f, 1f, 0f, 0f,
            0f, 0f, 0f, 1f, 0f,
            0f, 0f, 0f, 0f, 1f
        );

        /// <summary>
        /// Returns a new matrix with the negated elements of the given matrix.
        /// </summary>
        /// <param name="value">The source matrix.</param>
        /// <returns>The negated matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix5x5 operator -(Matrix5x5 value)
        {
            Matrix5x5 m;

            m.M11 = -value.M11;
            m.M12 = -value.M12;
            m.M13 = -value.M13;
            m.M14 = -value.M14;
            m.M15 = -value.M15;
            m.M21 = -value.M21;
            m.M22 = -value.M22;
            m.M23 = -value.M23;
            m.M24 = -value.M24;
            m.M25 = -value.M25;
            m.M31 = -value.M31;
            m.M32 = -value.M32;
            m.M33 = -value.M33;
            m.M34 = -value.M34;
            m.M35 = -value.M35;
            m.M41 = -value.M41;
            m.M42 = -value.M42;
            m.M43 = -value.M43;
            m.M44 = -value.M44;
            m.M45 = -value.M45;
            m.M51 = -value.M51;
            m.M52 = -value.M52;
            m.M53 = -value.M53;
            m.M54 = -value.M54;
            m.M55 = -value.M55;

            return m;
        }

        /// <summary>
        /// Subtracts the second matrix from the first.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The result of the subtraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix5x5 operator -(Matrix5x5 value1, Matrix5x5 value2)
        {
            Matrix5x5 m;

            m.M11 = value1.M11 - value2.M11;
            m.M12 = value1.M12 - value2.M12;
            m.M13 = value1.M13 - value2.M13;
            m.M14 = value1.M14 - value2.M14;
            m.M15 = value1.M15 - value2.M15;
            m.M21 = value1.M21 - value2.M21;
            m.M22 = value1.M22 - value2.M22;
            m.M23 = value1.M23 - value2.M23;
            m.M24 = value1.M24 - value2.M24;
            m.M25 = value1.M25 - value2.M25;
            m.M31 = value1.M31 - value2.M31;
            m.M32 = value1.M32 - value2.M32;
            m.M33 = value1.M33 - value2.M33;
            m.M34 = value1.M34 - value2.M34;
            m.M35 = value1.M35 - value2.M35;
            m.M41 = value1.M41 - value2.M41;
            m.M42 = value1.M42 - value2.M42;
            m.M43 = value1.M43 - value2.M43;
            m.M44 = value1.M44 - value2.M44;
            m.M45 = value1.M45 - value2.M45;
            m.M51 = value1.M51 - value2.M51;
            m.M52 = value1.M52 - value2.M52;
            m.M53 = value1.M53 - value2.M53;
            m.M54 = value1.M54 - value2.M54;
            m.M55 = value1.M55 - value2.M55;

            return m;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given two matrices are not equal.
        /// </summary>
        /// <param name="value1">The first matrix to compare.</param>
        /// <param name="value2">The second matrix to compare.</param>
        /// <returns>True if the given matrices are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Matrix5x5 value1, Matrix5x5 value2)
        {
            return value1.M11 != value2.M11 || value1.M12 != value2.M12 || value1.M13 != value2.M13 || value1.M14 != value2.M14 || value1.M15 != value2.M15 ||
                   value1.M21 != value2.M21 || value1.M22 != value2.M22 || value1.M23 != value2.M23 || value1.M24 != value2.M24 || value1.M25 != value2.M25 ||
                   value1.M31 != value2.M31 || value1.M32 != value2.M32 || value1.M33 != value2.M33 || value1.M34 != value2.M34 || value1.M35 != value2.M35 ||
                   value1.M41 != value2.M41 || value1.M42 != value2.M42 || value1.M43 != value2.M43 || value1.M44 != value2.M44 || value1.M45 != value2.M45 ||
                   value1.M51 != value2.M51 || value1.M52 != value2.M52 || value1.M53 != value2.M53 || value1.M54 != value2.M54 || value1.M55 != value2.M55;
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The result of the multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix5x5 operator *(Matrix5x5 value1, Matrix5x5 value2)
        {
            Matrix5x5 m;

            // First row
            m.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41 + value1.M15 * value2.M51;
            m.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42 + value1.M15 * value2.M52;
            m.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43 + value1.M15 * value2.M53;
            m.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44 + value1.M15 * value2.M54;
            m.M15 = value1.M11 * value2.M15 + value1.M12 * value2.M25 + value1.M13 * value2.M35 + value1.M14 * value2.M45 + value1.M15 * value2.M55;

            // Second row
            m.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41 + value1.M25 * value2.M51;
            m.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42 + value1.M25 * value2.M52;
            m.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43 + value1.M25 * value2.M53;
            m.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44 + value1.M25 * value2.M54;
            m.M25 = value1.M21 * value2.M15 + value1.M22 * value2.M25 + value1.M23 * value2.M35 + value1.M24 * value2.M45 + value1.M25 * value2.M55;

            // Third row
            m.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41 + value1.M35 * value2.M51;
            m.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42 + value1.M35 * value2.M52;
            m.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43 + value1.M35 * value2.M53;
            m.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44 + value1.M35 * value2.M54;
            m.M35 = value1.M31 * value2.M15 + value1.M32 * value2.M25 + value1.M33 * value2.M35 + value1.M34 * value2.M45 + value1.M35 * value2.M55;

            // Fourth row
            m.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41 + value1.M45 * value2.M51;
            m.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42 + value1.M45 * value2.M52;
            m.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43 + value1.M45 * value2.M53;
            m.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44 + value1.M45 * value2.M54;
            m.M45 = value1.M41 * value2.M15 + value1.M42 * value2.M25 + value1.M43 * value2.M35 + value1.M44 * value2.M45 + value1.M45 * value2.M55;

            // Fifth row
            m.M51 = value1.M51 * value2.M11 + value1.M52 * value2.M21 + value1.M53 * value2.M31 + value1.M54 * value2.M41 + value1.M55 * value2.M51;
            m.M52 = value1.M51 * value2.M12 + value1.M52 * value2.M22 + value1.M53 * value2.M32 + value1.M54 * value2.M42 + value1.M55 * value2.M52;
            m.M53 = value1.M51 * value2.M13 + value1.M52 * value2.M23 + value1.M53 * value2.M33 + value1.M54 * value2.M43 + value1.M55 * value2.M53;
            m.M54 = value1.M51 * value2.M14 + value1.M52 * value2.M24 + value1.M53 * value2.M34 + value1.M54 * value2.M44 + value1.M55 * value2.M54;
            m.M55 = value1.M51 * value2.M15 + value1.M52 * value2.M25 + value1.M53 * value2.M35 + value1.M54 * value2.M45 + value1.M55 * value2.M55;

            return m;
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="value1">The source matrix.</param>
        /// <param name="value2">The scaling factor.</param>
        /// <returns>The scaled matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix5x5 operator *(Matrix5x5 value1, float value2)
        {
            Matrix5x5 m;

            m.M11 = value1.M11 * value2;
            m.M12 = value1.M12 * value2;
            m.M13 = value1.M13 * value2;
            m.M14 = value1.M14 * value2;
            m.M15 = value1.M15 * value2;
            m.M21 = value1.M21 * value2;
            m.M22 = value1.M22 * value2;
            m.M23 = value1.M23 * value2;
            m.M24 = value1.M24 * value2;
            m.M25 = value1.M25 * value2;
            m.M31 = value1.M31 * value2;
            m.M32 = value1.M32 * value2;
            m.M33 = value1.M33 * value2;
            m.M34 = value1.M34 * value2;
            m.M35 = value1.M35 * value2;
            m.M41 = value1.M41 * value2;
            m.M42 = value1.M42 * value2;
            m.M43 = value1.M43 * value2;
            m.M44 = value1.M44 * value2;
            m.M45 = value1.M45 * value2;
            m.M51 = value1.M51 * value2;
            m.M52 = value1.M52 * value2;
            m.M53 = value1.M53 * value2;
            m.M54 = value1.M54 * value2;
            m.M55 = value1.M55 * value2;
            return m;
        }

        /// <summary>
        /// Multiplies a matrix by a float value.
        /// </summary>
        /// <param name="value1">The source matrix</param>
        /// <param name="value2">The vector</param>
        /// <returns>The resulting vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector4 operator *(Matrix5x5 value1, byte* value2)
        {
            float r = *value2 / 255f;
            float g = *(value2 + 1) / 255f;
            float b = *(value2 + 2) / 255f;
            float a = *(value2 + 3) / 255f;
            return new Vector4(r * value1.M11 + g * value1.M21 + b * value1.M31 + a * value1.M41 + value1.M51,
                                r * value1.M12 + g * value1.M22 + b * value1.M32 + a * value1.M42 + value1.M52,
                                r * value1.M13 + g * value1.M23 + b * value1.M33 + a * value1.M43 + value1.M53,
                                r * value1.M14 + g * value1.M24 + b * value1.M34 + a * value1.M44 + value1.M54);
        }

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="value1">The first source matrix.</param>
        /// <param name="value2">The second source matrix.</param>
        /// <returns>The resulting matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix5x5 operator +(Matrix5x5 value1, Matrix5x5 value2)
        {
            Matrix5x5 m;

            m.M11 = value1.M11 + value2.M11;
            m.M12 = value1.M12 + value2.M12;
            m.M13 = value1.M13 + value2.M13;
            m.M14 = value1.M14 + value2.M14;
            m.M15 = value1.M15 + value2.M15;
            m.M21 = value1.M21 + value2.M21;
            m.M22 = value1.M22 + value2.M22;
            m.M23 = value1.M23 + value2.M23;
            m.M24 = value1.M24 + value2.M24;
            m.M25 = value1.M25 + value2.M25;
            m.M31 = value1.M31 + value2.M31;
            m.M32 = value1.M32 + value2.M32;
            m.M33 = value1.M33 + value2.M33;
            m.M34 = value1.M34 + value2.M34;
            m.M35 = value1.M35 + value2.M35;
            m.M41 = value1.M41 + value2.M41;
            m.M42 = value1.M42 + value2.M42;
            m.M43 = value1.M43 + value2.M43;
            m.M44 = value1.M44 + value2.M44;
            m.M45 = value1.M45 + value2.M45;
            m.M51 = value1.M51 + value2.M51;
            m.M52 = value1.M52 + value2.M52;
            m.M53 = value1.M53 + value2.M53;
            m.M54 = value1.M54 + value2.M54;
            m.M55 = value1.M55 + value2.M55;

            return m;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given two matrices are equal.
        /// </summary>
        /// <param name="value1">The first matrix to compare.</param>
        /// <param name="value2">The second matrix to compare.</param>
        /// <returns>True if the given matrices are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Matrix5x5 value1, Matrix5x5 value2)
        {
            return value1.M11 == value2.M11 && value1.M22 == value2.M22 && value1.M33 == value2.M33 && value1.M44 == value2.M44 && value1.M55 == value2.M55 && // Check diagonal element first for early out.
                   value1.M12 == value2.M12 && value1.M13 == value2.M13 && value1.M14 == value2.M14 && value1.M15 == value2.M15 &&
                   value1.M21 == value2.M21 && value1.M23 == value2.M23 && value1.M24 == value2.M24 && value1.M25 == value2.M25 &&
                   value1.M31 == value2.M31 && value1.M32 == value2.M32 && value1.M34 == value2.M34 && value1.M35 == value2.M35 &&
                   value1.M41 == value2.M41 && value1.M42 == value2.M42 && value1.M43 == value2.M43 && value1.M45 == value2.M45 &&
                   value1.M51 == value2.M51 && value1.M52 == value2.M52 && value1.M53 == value2.M53 && value1.M54 == value2.M54;
        }

        /// <summary>
        /// Returns a boolean indicating whether this matrix instance is equal to the other given matrix.
        /// </summary>
        /// <param name="other">The matrix to compare this instance to.</param>
        /// <returns>True if the matrices are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Matrix5x5 other)
        {
            return M11 == other.M11 && M22 == other.M22 && M33 == other.M33 && M44 == other.M44 && M55 == other.M55 &&
                   M12 == other.M12 && M13 == other.M13 && M14 == other.M14 && M15 == other.M15 &&
                   M21 == other.M21 && M23 == other.M23 && M24 == other.M24 && M25 == other.M25 &&
                   M31 == other.M31 && M32 == other.M32 && M34 == other.M34 && M35 == other.M35 &&
                   M41 == other.M41 && M42 == other.M42 && M43 == other.M43 && M45 == other.M45 &&
                   M51 == other.M51 && M52 == other.M52 && M53 == other.M53 && M54 == other.M54;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this matrix instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this matrix; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is Matrix5x5)
            {
                return Equals((Matrix5x5)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() + M15.GetHashCode() +
                   M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() + M25.GetHashCode() +
                   M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() + M35.GetHashCode() +
                   M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode() + M45.GetHashCode() +
                   M51.GetHashCode() + M52.GetHashCode() + M53.GetHashCode() + M54.GetHashCode() + M55.GetHashCode();
        }

        /// <summary>
        /// Returns a String representing this matrix instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            CultureInfo ci = CultureInfo.CurrentCulture;

            return string.Format(ci, "{{ {{M11:{0} M12:{1} M13:{2} M14:{3} M15:{4}}} {{M21:{5} M22:{6} M23:{7} M24:{8} M25:{9}}} {{M31:{10} M32:{11} M33:{12} M34:{13} M35:{14}}} {{M41:{15} M42:{16} M43:{17} M44:{18} M45:{19}}} {{M51:{20} M52:{21} M53:{22} M54:{23} M55:{24}}} }}",
                                 M11.ToString(ci), M12.ToString(ci), M13.ToString(ci), M14.ToString(ci), M15.ToString(ci),
                                 M21.ToString(ci), M22.ToString(ci), M23.ToString(ci), M24.ToString(ci), M25.ToString(ci),
                                 M31.ToString(ci), M32.ToString(ci), M33.ToString(ci), M34.ToString(ci), M35.ToString(ci),
                                 M41.ToString(ci), M42.ToString(ci), M43.ToString(ci), M44.ToString(ci), M45.ToString(ci),
                                 M51.ToString(ci), M52.ToString(ci), M53.ToString(ci), M54.ToString(ci), M55.ToString(ci));
        }
    }
}