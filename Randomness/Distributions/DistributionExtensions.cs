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

        public static IDistribution<TResult> SelectMany<TSource, TLikelihood, TResult>(
            this IDistribution<TSource> prior,
            Func<TSource, IDistribution<TLikelihood>> likelihood,
            Func<TSource, TLikelihood, TResult> projection)
        {
            return Combined<TSource, TLikelihood, TResult>.Distribution(prior, likelihood, projection);
        }
    }
}