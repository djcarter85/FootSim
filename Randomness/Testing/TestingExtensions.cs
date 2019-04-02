namespace Randomness.Testing
{
    using System.Collections.Generic;
    using System.Linq;
    using Distributions;

    public static class TestingExtensions
    {
        public static string Histogram(this IDistribution<double> distribution, double low, double high) =>
            distribution.Samples().Histogram(low, high);

        public static string Histogram<T>(this IDiscreteDistribution<T> d) =>
            d.Samples().DiscreteHistogram();

        public static string ShowWeights<T>(this IDiscreteDistribution<T> d)
        {
            int labelMax = d.Support()
                .Select(x => x.ToString().Length)
                .Max();

            return d.Support()
                .Select(s => $"{ToLabel(s)}:{d.Weight(s)}")
                .Join("\n");

            string ToLabel(T t) => t.ToString().PadLeft(labelMax);
        }

        public static string Histogram(this IEnumerable<double> doubles, double low, double high)
        {
            const int width = 40;
            const int height = 20;
            const int sampleCount = 100000;
            var buckets = new int[width];

            foreach (var c in doubles.Take(sampleCount))
            {
                int bucket = (int)(buckets.Length * (c - low) / (high - low));

                if (0 <= bucket && bucket < buckets.Length)
                {
                    buckets[bucket] += 1;
                }
            }

            var max = buckets.Max();
            var scale = max < height ? 1.0 : ((double)height) / max;

            var bars = Enumerable.Range(0, height)
                .Select(r =>
                    buckets
                        .Select(b => b * scale > (height - r) ? '*' : ' ')
                        .Join() +
                    "\n")
                .Join();

            return bars + new string('-', width) + "\n";
        }

        public static string DiscreteHistogram<T>(this IEnumerable<T> d)
        {
            const int sampleCount = 100000;
            const int width = 40;

            var dict = d.Take(sampleCount)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            int labelMax = dict.Keys
                .Select(x => x.ToString().Length)
                .Max();

            var sup = dict.Keys.OrderBy(ToLabel).ToList();

            int max = dict.Values.Max();

            double scale = max < width ? 1.0 : ((double)width) / max;

            return sup.Select(s => $"{ToLabel(s)}|{Bar(s)}").Join("\n");

            string ToLabel(T t) => t.ToString().PadLeft(labelMax);

            string Bar(T t) => new string('*', (int)(dict[t] * scale));
        }

        public static string Join(this IEnumerable<string> source) => string.Join("", source);

        public static string Join(this IEnumerable<string> source, string separator) => string.Join(separator, source);

        public static string Join(this IEnumerable<char> source) => string.Join("", source);
    }
}