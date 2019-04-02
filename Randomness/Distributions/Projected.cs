namespace Randomness.Distributions
{
    using System;

    public sealed class Projected<A, R> : IDistribution<R>
    {
        private readonly IDistribution<A> underlying;
        private readonly Func<A, R> projection;

        public static IDistribution<R> Distribution(
            IDistribution<A> underlying,
            Func<A, R> projection)
        {
            return new Projected<A, R>(underlying, projection);
        }

        private Projected(IDistribution<A> underlying, Func<A, R> projection)
        {
            this.underlying = underlying;
            this.projection = projection;
        }

        public R Sample() => this.projection(this.underlying.Sample());
    }
}