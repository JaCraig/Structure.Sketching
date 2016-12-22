using BenchmarkDotNet.Attributes;

namespace Structure.Sketching.Benchmarks.GenericSpeedTests
{
    public class ColorTests
    {
        [Benchmark(Description = "New Color struct")]
        public void NewColorStruct()
        {
            var TestArray = new TestClasses.ColorStruct[10000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x] *= 3;
            }
        }

        [Benchmark(Baseline = true, Description = "Old Color struct")]
        public void OldColorStruct()
        {
            var TestArray = new Colors.Color[10000];
            for (int x = 0; x < TestArray.Length; ++x)
            {
                TestArray[x] *= 3;
            }
        }
    }
}