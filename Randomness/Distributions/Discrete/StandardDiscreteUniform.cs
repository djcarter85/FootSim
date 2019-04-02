namespace Randomness.Distributions.Discrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SCU = Continuous.StandardContinuousUniform;

    public sealed class StandardDiscreteUniform : IDiscreteDistribution<int>
    {
        private StandardDiscreteUniform(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public int Min { get; }

        public int Max { get; }

        public static StandardDiscreteUniform Distribution(int min, int max)
        {
            if (min > max)
                throw new ArgumentException();
            return new StandardDiscreteUniform(min, max);
        }

        public IEnumerable<int> Support() => Enumerable.Range(this.Min, 1 + this.Max - this.Min);

        public int Sample() => (int)(SCU.Distribution.Sample() * (1.0 + this.Max - this.Min)) + this.Min;

        public int Weight(int i) => (this.Min <= i && i <= this.Max) ? 1 : 0;

        public override string ToString() => $"StandardDiscreteUniform[{this.Min}, {this.Max}]";
    }
}