namespace Randomness.Distributions
{
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
    }
}