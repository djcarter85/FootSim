namespace FootballPredictor
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandLine;
    using FootballPredictor.Core;

    public static class Program
    {
        private class Arguments
        {
            [Option('r', "refresh", Required = false, HelpText = "Refresh the results from www.football-data.co.uk.")]
            public bool Refresh { get; set; }
        }

        public static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Arguments>(args)
                .WithParsed(async o => { await Run(o); });
        }

        private static async Task Run(Arguments arguments)
        {
            var repository = new Repository(Constants.CsvFilePath, Constants.Url, lastDate: null);

            if (arguments.Refresh)
            {
                await repository.RefreshFromWebAsync();
            }

            RunSimulations(repository);

            Console.ReadLine();
        }

        private static void RunSimulations(Repository repository)
        {
            var simulations = 10_000;

            Console.WriteLine($"Simulating {simulations} seasons ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator(repository);
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
        }

        private static string GetDescription(SeasonSimulationResult seasonSimulationResult)
        {
            var stringBuilder = new StringBuilder();

            foreach (var position in Enumerable.Range(1, 20))
            {
                var proportion = seasonSimulationResult.PositionProportion(position);
                var percentage = (proportion * 100).ToString("N1");

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
