using System;

namespace Structure.Sketching.Numerics
{
    /// <summary>
    /// Random class
    /// </summary>
    /// <seealso cref="System.Random"/>
    public class Random : System.Random
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Random"/> class.
        /// </summary>
        public Random()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Random"/> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public Random(int seed)
            : base(seed)
        {
        }

        /// <summary>
        /// The global seed
        /// </summary>
        private static readonly Random GlobalSeed = new Random();

        /// <summary>
        /// The thread static local random object
        /// </summary>
        [ThreadStatic]
        private static Random Local;

        /// <summary>
        /// A thread safe version of a random number generation
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>A randomly generated value</returns>
        public static int ThreadSafeNext(int min = int.MinValue, int max = int.MaxValue)
        {
            if (min > max)
            {
                var temp = min;
                min = max;
                max = min;
            }
            if (Local == null)
            {
                int Seed;
                lock (GlobalSeed)
                    Seed = GlobalSeed.Next();
                Local = new Random(Seed);
            }
            return Local.Next(min, max);
        }

        /// <summary>
        /// A thread safe version of a random number generation
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>A randomly generated value</returns>
        public static double ThreadSafeNextDouble(double min = double.MinValue, double max = double.MaxValue)
        {
            if (min > max)
            {
                var temp = min;
                min = max;
                max = min;
            }
            if (Local == null)
            {
                int Seed;
                lock (GlobalSeed)
                    Seed = GlobalSeed.Next();
                Local = new Random(Seed);
            }
            return min + ((max - min) * Local.NextDouble());
        }
    }
}