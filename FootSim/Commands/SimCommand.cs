namespace FootSim.Commands
{
    using System;
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
            var repository = new Repository(options.Season.ForWeb(), options.League.ForWeb());

            Console.WriteLine($"League: {options.League.ForDisplay()}");
            Console.WriteLine($"Season: {options.Season.ForDisplay()}");
            Console.WriteLine();
            Console.WriteLine($"Simulating {options.Times:N0} times ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator(repository);
            var result = seasonSimulator.Simulate(options.Times, options.On);

            stopwatch.Stop();

            var tableBuilder = CreateTableBuilder();

            Console.WriteLine();
            Console.WriteLine(tableBuilder.Build(result.Teams));

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            return ExitCode.Success;
        }

        private static TableBuilder<TeamSeasonSimulationResult> CreateTableBuilder()
        {
            var tableBuilder = new TableBuilder<TeamSeasonSimulationResult>();

            tableBuilder.AddColumn(
                "Name",
                20,
                Alignment.Left,
                tssr => tssr.TeamName);

            foreach (var position in Enumerable.Range(1, 20))
            {
                tableBuilder.AddColumn(
                    $"#{position}",
                    5,
                    Alignment.Right,
                    tssr => CalculatePercentage(position, tssr));
            }

            tableBuilder.AddColumn(
                "Avg Pts",
                8,
                Alignment.Right,
                tssr => tssr.AveragePoints.ToString("N1"));

            return tableBuilder;
        }

        private static string CalculatePercentage(int position, TeamSeasonSimulationResult teamSeasonSimulationResult)
        {
            var positionCount = teamSeasonSimulationResult.PositionCount(position);
            var percentage = positionCount == 0
                ? string.Empty
                : ((double)positionCount / teamSeasonSimulationResult.Positions.Count * 100).ToString(".0");

            return percentage;
        }
    }
}