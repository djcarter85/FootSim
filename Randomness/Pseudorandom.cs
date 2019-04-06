namespace Randomness
{
    using System;
    using System.Threading;

    public static class Pseudorandom
    {
        private static readonly ThreadLocal<Random> Random = new ThreadLocal<Random>(() => new Random(BetterRandom.NextInt()));

        public static double NextDouble() => Random.Value.NextDouble();
    }
}