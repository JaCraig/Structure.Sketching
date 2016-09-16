using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;

namespace Structure.Sketching.Benchmarks
{
    /// <summary>
    /// Benchmark config
    /// </summary>
    /// <seealso cref="BenchmarkDotNet.Configs.ManualConfig"/>
    public class Config : ManualConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public Config()
        {
            Add(new MemoryDiagnoser());
        }
    }
}