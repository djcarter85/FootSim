namespace Randomness.Distributions
{
    using System;

    public class Poisson : IDistribution<int>
    {
        private readonly double lambda;

        private Poisson(double lambda)
        {
            this.lambda = lambda;
        }

        public static Poisson Distribution(double lambda) => new Poisson(lambda);

        public int Sample()
        {
            var l = Math.Exp(-this.lambda);
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