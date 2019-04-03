namespace Randomness.Distributions
{
    using System;

    public sealed class Combined<A, B, C> : IDistribution<C>
    {
        private readonly IDistribution<A> prior;
        private readonly Func<A, IDistribution<B>> likelihood;
        private readonly Func<A, B, C> projection;

        public static IDistribution<C> Distribution(
            IDistribution<A> prior,
            Func<A, IDistribution<B>> likelihood,
            Func<A, B, C> projection) =>
            new Combined<A, B, C>(prior, likelihood, projection);

        private Combined(
            IDistribution<A> prior,
            Func<A, IDistribution<B>> likelihood,
            Func<A, B, C> projection)
        {
            this.prior = prior;
            this.likelihood = likelihood;
            this.projection = projection;
        }

        public C Sample()
        {
            A a = this.prior.Sample();
            B b = this.likelihood(a).Sample();
            return this.projection(a, b);
        }
    }
}