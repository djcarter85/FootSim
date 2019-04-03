namespace FootballPredictor
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using FootballPredictor.Core;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var simulations = 10_000;

            Console.WriteLine($"Simulating {simulations} seasons ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = CreateSeasonSimulator();
            var results = seasonSimulator.Simulate(simulations);

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(GetHeaderLine());

            foreach (var keyValuePair in results.OrderByDescending(kvp => kvp.Value.AveragePoints))
            {
                var teamName = keyValuePair.Key;

                Console.WriteLine($"{teamName,-20} {GetDescription(keyValuePair.Value)}");
            }

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            Console.ReadLine();
        }

        private static SeasonSimulator CreateSeasonSimulator()
        {
            return new SeasonSimulator(new Repository(Constants.CsvFilePath));
        }

        private static string GetDescription(SeasonSimulationResult seasonSimulationResult)
        {
            var stringBuilder = new StringBuilder();

            foreach (var position in Enumerable.Range(1, 20))
            {
                var value = seasonSimulationResult.PositionPercentage(position);
                var percentage = (value * 100).ToString("N1");

                stringBuilder.Append($"{percentage,5} ");
            }

            var avgPts = seasonSimulationResult.AveragePoints.ToString("N1");
            stringBuilder.Append($"{avgPts,8}");

            return stringBuilder.ToString();
        }

        private static string GetHeaderLine()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"{"Name",-20} ");

            foreach (var position in Enumerable.Range(1, 20))
            {
                var pos = $"#{position}";
                stringBuilder.Append($"{pos,5} ");
            }

            stringBuilder.Append(" Avg Pts");

            return stringBuilder.ToString();
        }
    }
}
