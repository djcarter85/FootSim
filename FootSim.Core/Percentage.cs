namespace FootSim.Core
{
    using System;

    public class Percentage : IComparable<Percentage>
    {
        public int CompareTo(Percentage other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return this.Proportion.CompareTo(other.Proportion);
        }

        private Percentage(decimal proportion)
        {
            this.Proportion = proportion;
        }

        public decimal Proportion { get; }

        public static Percentage FromFraction(int numerator, int denominator) =>
            new Percentage((decimal)numerator / denominator);

        public override string ToString()
        {
            return $"{this.Proportion *100:N2}%";
        }
    }
}