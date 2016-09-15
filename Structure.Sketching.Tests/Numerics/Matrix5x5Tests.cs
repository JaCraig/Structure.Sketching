using Structure.Sketching.Numerics;
using Xunit;

namespace Structure.Sketching.Tests.Numerics
{
    public class Matrix5x5Tests
    {
        [Fact]
        public void Addition()
        {
            var first = new Matrix5x5(
                1, 2, 3, 4, 6,
                6, 1, 5, 3, 8,
                2, 6, 4, 9, 9,
                1, 3, 8, 3, 4,
                5, 7, 8, 2, 5
            );
            var second = new Matrix5x5(
                3, 5, 0, 8, 7,
                2, 2, 4, 8, 3,
                0, 2, 5, 1, 2,
                1, 4, 0, 5, 1,
                3, 4, 8, 2, 3
            );
            var result = new Matrix5x5(
                4, 7, 3, 12, 13,
                8, 3, 9, 11, 11,
                2, 8, 9, 10, 11,
                2, 7, 8, 8, 5,
                8, 11, 16, 4, 8
            );
            Assert.Equal(result, first + second);
        }

        /// <summary>
        /// Tests the equality operators for equality.
        /// </summary>
        [Fact]
        public void AreEqual()
        {
            var first = new Matrix5x5(
                1, 0, 0, 0, 0,
                0, 1, 0, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 0, 1, 0,
                0, 0, 0, 0, 1
            );
            var second = new Matrix5x5(
                1, 0, 0, 0, 0,
                0, 1, 0, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 0, 1, 0,
                0, 0, 0, 0, 1
            );
            Assert.Equal(first, second);
        }

        /// <summary>
        /// Tests the equality operators for inequality.
        /// </summary>
        [Fact]
        public void AreNotEqual()
        {
            var first = new Matrix5x5(
                1, 0, 0, 0, 0,
                0, 1, 0, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 0, 1, 0,
                0, 0, 0, 0, 1
            );
            var second = new Matrix5x5(
                1, 0, 0, 0, 1,
                0, 1, 0, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 0, 1, 0,
                0, 0, 0, 0, 1
            );
            Assert.NotEqual(first, second);
        }

        /// <summary>
        /// Tests whether the matrix constructor correctly assign properties.
        /// </summary>
        [Fact]
        public void ConstructorAssignsProperties()
        {
            var value = new Matrix5x5(
                1, 0, 0, 0, 0,
                0, 1, 0, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 0, 1, 0,
                0, 0, 0, 0, 1
            );
            Assert.Equal(1, value.M11);
            Assert.Equal(1, value.M22);
            Assert.Equal(1, value.M33);
            Assert.Equal(1, value.M44);
            Assert.Equal(1, value.M55);
            Assert.Equal(0, value.M12);
            Assert.Equal(0, value.M13);
            Assert.Equal(0, value.M14);
            Assert.Equal(0, value.M15);
            Assert.Equal(0, value.M21);
            Assert.Equal(0, value.M23);
            Assert.Equal(0, value.M24);
            Assert.Equal(0, value.M25);
            Assert.Equal(0, value.M31);
            Assert.Equal(0, value.M32);
            Assert.Equal(0, value.M34);
            Assert.Equal(0, value.M35);
            Assert.Equal(0, value.M41);
            Assert.Equal(0, value.M42);
            Assert.Equal(0, value.M43);
            Assert.Equal(0, value.M45);
            Assert.Equal(0, value.M51);
            Assert.Equal(0, value.M52);
            Assert.Equal(0, value.M53);
            Assert.Equal(0, value.M54);
        }

        [Fact]
        public void IsIdentity()
        {
            var value = new Matrix5x5(
                1, 0, 0, 0, 0,
                0, 1, 0, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 0, 1, 0,
                0, 0, 0, 0, 1
            );
            Assert.Equal(Matrix5x5.Identity, value);
        }

        [Fact]
        public void Minus()
        {
            var first = new Matrix5x5(
                1, 2, 3, 4, 6,
                6, 1, 5, 3, 8,
                2, 6, 4, 9, 9,
                1, 3, 8, 3, 4,
                5, 7, 8, 2, 5
            );
            var second = new Matrix5x5(
                3, 5, 0, 8, 7,
                2, 2, 4, 8, 3,
                0, 2, 5, 1, 2,
                1, 4, 0, 5, 1,
                3, 4, 8, 2, 3
            );
            var result = new Matrix5x5(
                -2, -3, 3, -4, -1,
                4, -1, 1, -5, 5,
                2, 4, -1, 8, 7,
                0, -1, 8, -2, 3,
                2, 3, 0, 0, 2
            );
            Assert.Equal(result, first - second);
        }

        [Fact]
        public void Multiplication()
        {
            var first = new Matrix5x5(
                1, 2, 3, 4, 6,
                6, 1, 5, 3, 8,
                2, 6, 4, 9, 9,
                1, 3, 8, 3, 4,
                5, 7, 8, 2, 5
            );
            var second = new Matrix5x5(
                3, 5, 0, 8, 7,
                2, 2, 4, 8, 3,
                0, 2, 5, 1, 2,
                1, 4, 0, 5, 1,
                3, 4, 8, 2, 3
            );
            var result = new Matrix5x5(
                29, 55, 71, 59, 41,
                47, 86, 93, 92, 82,
                54, 102, 116, 131, 76,
                24, 55, 84, 63, 47,
                46, 83, 108, 124, 89
            );
            Assert.Equal(result, first * second);
        }

        [Fact]
        public void MultiplicationFloat()
        {
            var first = new Matrix5x5(
                1, 2, 3, 4, 6,
                6, 1, 5, 3, 8,
                2, 6, 4, 9, 9,
                1, 3, 8, 3, 4,
                5, 7, 8, 2, 5
            );
            float value = 2;
            var result = new Matrix5x5(
                2, 4, 6, 8, 12,
                12, 2, 10, 6, 16,
                4, 12, 8, 18, 18,
                2, 6, 16, 6, 8,
                10, 14, 16, 4, 10
            );
            Assert.Equal(result, first * value);
        }

        [Fact]
        public void Negation()
        {
            var first = new Matrix5x5(
                1, 2, 3, 4, 6,
                6, 1, 5, 3, 8,
                2, 6, 4, 9, 9,
                1, 3, 8, 3, 4,
                5, 7, 8, 2, 5
            );
            var result = new Matrix5x5(
                -1, -2, -3, -4, -6,
                -6, -1, -5, -3, -8,
                -2, -6, -4, -9, -9,
                -1, -3, -8, -3, -4,
                -5, -7, -8, -2, -5
            );
            Assert.Equal(result, -first);
        }
    }
}