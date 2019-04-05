namespace FootSim.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using FootSim.Core;
    using FootSim.Options;
    using FootSim.Table;

    public static class SimCommand
    {
        public static async Task<ExitCode> RunAsync(SimOptions options)
        {
            var repository = new Repository(options.Season);

            Console.WriteLine($"Simulating the {options.Season} season {options.Times:N0} times ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator(repository);
            var results = seasonSimulator.Simulate(options.Times, options.On);

            stopwatch.Stop();

            var tableBuilder = CreateTableBuilder();

            Console.WriteLine();
            Console.WriteLine(tableBuilder.Build(results));

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            Console.ReadLine();

            return ExitCode.Success;
        }

        private static TableBuilder<KeyValuePair<string, SeasonSimulationResult>> CreateTableBuilder()
        {
            var tableBuilder = new TableBuilder<KeyValuePair<string, SeasonSimulationResult>>();

            tableBuilder.AddColumn(
                "Name",
                20,
                Alignment.Left,
                kvp => kvp.Key);

            foreach (var position in Enumerable.Range(1, 20))
            {
                tableBuilder.AddColumn(
                    $"#{position}",
                    5,
                    Alignment.Right,
                    kvp => CalculatePercentage(position, kvp.Value));
            }

            tableBuilder.AddColumn(
                "Avg Pts",
                8,
                Alignment.Right,
                kvp => kvp.Value.AveragePoints.ToString("N1"));

            return tableBuilder;
        }

        private static string CalculatePercentage(int position, SeasonSimulationResult seasonSimulationResult)
        {
            var positionCount = seasonSimulationResult.PositionCount(position);
            var percentage = positionCount == 0
                ? string.Empty
                : ((double)positionCount / seasonSimulationResult.Positions.Count * 100).ToString(".0");

            return percentage;
        }
    }
}