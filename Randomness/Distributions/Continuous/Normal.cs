namespace Randomness.Distributions.Continuous
{
    using static System.Math;
    using SCU = StandardContinuousUniform;

    public sealed class Normal : IDistribution<double>
    {
        public static readonly Normal Standard = Distribution(0, 1);

        private Normal(double mean, double sigma)
        {
            this.Mean = mean;
            this.Sigma = sigma;
        }

        public double Mean { get; }

        public double Sigma { get; }

        public static Normal Distribution(double mean, double sigma)
        {
            return new Normal(mean, sigma);
        }

        private double StandardSample()
        {
            // Box-Muller method
            return Sqrt(-2.0 * Log(SCU.Distribution.Sample())) *
                   Cos(2.0 * PI * SCU.Distribution.Sample());
        }

        public double Sample() => this.Mean + this.Sigma * this.StandardSample();
    }
}