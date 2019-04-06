namespace Randomness.Distributions.Discrete
{
    using System;
    using Randomness.Distributions.Continuous;

    public class Poisson : IDistribution<int>
    {
        private Poisson(double lambda)
        {
            this.Lambda = lambda;
        }

        public double Lambda { get; }

        public static Poisson Distribution(double lambda) => new Poisson(lambda);

        public int Sample()
        {
            var l = Math.Exp(-this.Lambda);
            var k = 0;
            double p = 1;

            do
            {
                k++;
                var u = StandardContinuousUniform.Distribution.Sample();
                p *= u;

            } while (p > l);

            return k - 1;
        }
    }
}