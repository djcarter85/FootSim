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
            var repository = new Repository(options.Season.ForWeb(), options.League.ForWeb());

            Console.WriteLine($"League: {options.League.ForDisplay()}");
            Console.WriteLine($"Season: {options.Season.ForDisplay()}");
            Console.WriteLine();
            Console.WriteLine("League table:");
            Console.WriteLine();

            var seasonSoFar = new Season(repository.Matches(options.On));

            Console.WriteLine(CreateLeagueTable(seasonSoFar));

            Console.WriteLine();
            Console.WriteLine($"Simulating {options.Times:N0} times ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator(repository);
            var result = seasonSimulator.Simulate(options.Times, seasonSoFar);

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(CreateSimulationTable(result.Teams));

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            return ExitCode.Success;
        }

        private static string CreateSimulationTable(IReadOnlyList<TeamSeasonSimulationResult> teams)
        {
            var tableBuilder = new TableBuilder<TeamSeasonSimulationResult>();

            tableBuilder.AddColumn(
                "Name",
                teams.Select(t => t.TeamName.Length).Max() + 3,
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

            return tableBuilder.Build(teams);
        }

        private static string CreateLeagueTable(Season season)
        {
            var tableBuilder = new TableBuilder<TablePlacing>();

            tableBuilder.AddColumn("#", 2, Alignment.Right, tp => tp.Position);
            tableBuilder.AddColumn("Name", season.Table.Max(t => t.TeamName.Length), Alignment.Left, tp => tp.TeamName);
            tableBuilder.AddColumn("Pld", 4, Alignment.Right, tp => tp.Played);
            tableBuilder.AddColumn("W", 4, Alignment.Right, tp => tp.Won);
            tableBuilder.AddColumn("D", 4, Alignment.Right, tp => tp.Drawn);
            tableBuilder.AddColumn("L", 4, Alignment.Right, tp => tp.Lost);
            tableBuilder.AddColumn("GD", 4, Alignment.Right, tp => tp.GoalDifference);
            tableBuilder.AddColumn("Pts", 4, Alignment.Right, tp => tp.Points);

            return tableBuilder.Build(season.Table);
        }

        private static string CalculatePercentage(int position, TeamSeasonSimulationResult teamSeasonSimulationResult)
        {
            var positionCount = teamSeasonSimulationResult.PositionCount(position);

            return positionCount == 0
                ? string.Empty
                : ((double)positionCount / teamSeasonSimulationResult.Positions.Count * 100).ToString(".0");
        }
    }
}