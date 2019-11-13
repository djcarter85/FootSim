namespace FootSim.Core
{
    using System;

    public class Percentage : IComparable<Percentage>
    {
        public int CompareTo(Percentage other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return this.percentage.CompareTo(other.percentage);
        }

        private readonly decimal percentage;

        private Percentage(decimal percentage)
        {
            this.percentage = percentage;
        }

        public static Percentage FromFraction(int numerator, int denominator) =>
            new Percentage((decimal)numerator / denominator * 100);

        public override string ToString()
        {
            return $"{this.percentage:N2}%";
        }
    }
}