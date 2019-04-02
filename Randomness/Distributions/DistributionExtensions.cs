namespace Randomness.Distributions
{
    using System;
    using System.Collections.Generic;

    public static class DistributionExtensions
    {
        public static IEnumerable<T> Samples<T>(this IDistribution<T> distribution)
        {
            while (true)
            {
                yield return distribution.Sample();
            }
        }

        public static IDistribution<R> Select<A, R>(this IDistribution<A> distribution, Func<A, R> projection)
        {
            return Projected<A, R>.Distribution(distribution, projection);
        }
    }
}