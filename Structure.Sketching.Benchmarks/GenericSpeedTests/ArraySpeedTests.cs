using BenchmarkDotNet.Attributes;
using Structure.Sketching.Benchmarks.GenericSpeedTests.TestClasses;
using System.Numerics;

namespace Structure.Sketching.Benchmarks.ArraySpeedTests
{
    public class ArraySpeedTests
    {
        [Benchmark(Baseline = true, Description = "Byte array manipulation")]
        public void ByteArray()
        {
            var TestArray = new byte[40000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x] *= 3;
            }
        }

        [Benchmark(Description = "ColorStruct array manipulation")]
        public void ColorStructArray()
        {
            var TestArray = new ColorStruct[10000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x].Red *= 3;
                TestArray[x].Green *= 3;
                TestArray[x].Blue *= 3;
                TestArray[x].Alpha *= 3;
            }
        }

        [Benchmark(Description = "ColorStruct array manipulation 2")]
        public void ColorStructArray2()
        {
            var TestArray = new ColorStruct[10000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x] *= 3;
            }
        }

        [Benchmark(Description = "Float array manipulation")]
        public void FloatArray()
        {
            var TestArray = new float[40000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x] *= 3;
            }
        }

        [Benchmark(Description = "Unsigned int array manipulation")]
        public void UIntArray()
        {
            var TestArray = new uint[10000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x] = (uint)(((byte)((TestArray[x] & 0x00FF0000) * 3)) << 16
                            | ((byte)((TestArray[x] & 0x0000FF00) * 3) << 8)
                            | ((byte)((TestArray[x] & 0x000000FF) * 3))
                            | ((byte)((TestArray[x] & 0xFF000000) * 3) << 24));
            }
        }

        [Benchmark(Description = "Vector4 array manipulation")]
        public void Vector4Array()
        {
            var TestArray = new Vector4[10000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x] = TestArray[x] * 3;
            }
        }
    }
}