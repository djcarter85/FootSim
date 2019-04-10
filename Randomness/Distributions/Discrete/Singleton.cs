namespace Randomness.Distributions.Discrete
{
    public static class Singleton
    {
        public static IDistribution<T> Distribution<T>(T t) => new SingletonDistribution<T>(t);

        private sealed class SingletonDistribution<T> : IDistribution<T>
        {
            private readonly T t;

            public SingletonDistribution(T t) => this.t = t;

            public T Sample() => this.t;

            public override string ToString() => $"Singleton[{this.t}]";
        }
    }
}