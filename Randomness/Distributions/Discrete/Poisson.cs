namespace Randomness.Distributions.Discrete
{
    using static System.Math;
    using SCU = Continuous.StandardContinuousUniform;

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
            var l = Exp(-this.Lambda);
            var k = 0;
            double p = 1;

            do
            {
                k++;
                var u = SCU.Distribution.Sample();
                p *= u;

            } while (p > l);

            return k - 1;
        }
    }
}