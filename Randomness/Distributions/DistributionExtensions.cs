namespace Randomness.Distributions
{
    using System;
    using System.Collections.Generic;

    public static class DistributionExtensions
    {
        public static IEnumerable<T> TakeSamples<T>(this IDistribution<T> distribution, int samples)
        {
            for (var i = 0; i < samples; i++)
            {
                yield return distribution.Sample();
            }
        }

        public static IDistribution<R> Select<A, R>(this IDistribution<A> distribution, Func<A, R> projection)
        {
            return Projected<A, R>.Distribution(distribution, projection);
        }

        public static IDistribution<C> SelectMany<A, B, C>(
            this IDistribution<A> prior,
            Func<A, IDistribution<B>> likelihood,
            Func<A, B, C> projection)
        {
            return Combined<A, B, C>.Distribution(prior, likelihood, projection);
        }
    }
}