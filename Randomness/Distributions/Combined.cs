namespace Randomness.Distributions
{
    using System;

    public sealed class Combined<TSource, TLikelihood, TResult> : IDistribution<TResult>
    {
        private readonly IDistribution<TSource> prior;
        private readonly Func<TSource, IDistribution<TLikelihood>> likelihood;
        private readonly Func<TSource, TLikelihood, TResult> projection;

        public static IDistribution<TResult> Distribution(
            IDistribution<TSource> prior,
            Func<TSource, IDistribution<TLikelihood>> likelihood,
            Func<TSource, TLikelihood, TResult> projection) =>
            new Combined<TSource, TLikelihood, TResult>(prior, likelihood, projection);

        private Combined(
            IDistribution<TSource> prior,
            Func<TSource, IDistribution<TLikelihood>> likelihood,
            Func<TSource, TLikelihood, TResult> projection)
        {
            this.prior = prior;
            this.likelihood = likelihood;
            this.projection = projection;
        }

        public TResult Sample()
        {
            TSource a = this.prior.Sample();
            TLikelihood b = this.likelihood(a).Sample();
            return this.projection(a, b);
        }
    }
}