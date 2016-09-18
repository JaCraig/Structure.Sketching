using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace Structure.Sketching.Benchmarks.GenericSpeedTests
{
    public class ArrayCopyTests
    {
        [Params(100, 1000, 10000)]
        public int Count { get; set; }

        private long sizeInBytes;
        private byte[] source, destination;

        [Benchmark(Baseline = true, Description = "Copy using Array.Copy()")]
        public void CopyArray()
        {
            Array.Copy(source, destination, Count);
        }

        [Benchmark(Description = "Copy using Unsafe<T>")]
        public unsafe void CopyUnsafe()
        {
            fixed (byte* pinnedDestination = destination)
            fixed (byte* pinnedSource = source)
            {
                Unsafe.CopyBlock(pinnedSource, pinnedDestination, (uint)Count);
            }
        }

        [Benchmark(Description = "Copy using Buffer.MemoryCopy<T>")]
        public unsafe void CopyUsingBufferMemoryCopy()
        {
            fixed (byte* pinnedDestination = destination)
            fixed (byte* pinnedSource = source)
            {
                Buffer.MemoryCopy(pinnedSource, pinnedDestination, Count, Count);
            }
        }

        [Setup]
        public void SetUp()
        {
            source = new byte[Count];
            destination = new byte[Count];
        }
    }
}