namespace Randomness
{
    using System;
    using System.Security.Cryptography;
    using System.Threading;

    public static class BetterRandom
    {
        private static readonly ThreadLocal<RandomNumberGenerator> Crng = new ThreadLocal<RandomNumberGenerator>(RandomNumberGenerator.Create);
        private static readonly ThreadLocal<byte[]> Bytes = new ThreadLocal<byte[]>(() => new byte[sizeof(int)]);

        public static int NextInt()
        {
            Crng.Value.GetBytes(Bytes.Value);
            return BitConverter.ToInt32(Bytes.Value, 0) & int.MaxValue;
        }
    }
}
