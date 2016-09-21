using BenchmarkDotNet.Attributes;
using Structure.Sketching.ExtensionMethods;
using Structure.Sketching.Numerics;
using System.Numerics;

namespace Structure.Sketching.Benchmarks.GenericSpeedTests
{
    public class MatrixMultiplication
    {
        private Matrix5x5 value1 = new Matrix5x5(
            1, 0, 0, 0, 0,
            0, 1, 0, 0, 0,
            0, 0, 1, 0, 0,
            .5f, .5f, .5f, 0, 0,
            0, 0, 0, 0, 1
            );

        [Benchmark(Description = "Byte matrix multiplication")]
        public void ByteMatrixMultiplication()
        {
            float r = 128;
            float g = 127;
            float b = 126;
            float a = 255;
            var finalvalue = new byte[]
            {
                ((byte)(r* value1.M11 + g* value1.M21 + b* value1.M31 + a* value1.M41 + (value1.M51*255f)).Clamp(0,255)),
                ((byte)(r* value1.M12 + g* value1.M22 + b* value1.M32 + a* value1.M42 + (value1.M52*255f)).Clamp(0,255)),
                ((byte)(r* value1.M13 + g* value1.M23 + b* value1.M33 + a* value1.M43 + (value1.M53*255f)).Clamp(0,255)),
                ((byte)(r* value1.M14 + g* value1.M24 + b* value1.M34 + a* value1.M44 + (value1.M54*255f)).Clamp(0,255))
            };
        }

        [Benchmark(Description = "Float matrix multiplication")]
        public void FloatMatrixMultiplication()
        {
            float r = 128 / 255f;
            float g = 127 / 255f;
            float b = 126 / 255f;
            float a = 255 / 255f;
            var finalvalue = new Vector4(r * value1.M11 + g * value1.M21 + b * value1.M31 + a * value1.M41 + value1.M51,
                                r * value1.M12 + g * value1.M22 + b * value1.M32 + a * value1.M42 + value1.M52,
                                r * value1.M13 + g * value1.M23 + b * value1.M33 + a * value1.M43 + value1.M53,
                                r * value1.M14 + g * value1.M24 + b * value1.M34 + a * value1.M44 + value1.M54);
            finalvalue = Vector4.Clamp(finalvalue, Vector4.Zero, Vector4.One) * 255f;
        }
    }
}