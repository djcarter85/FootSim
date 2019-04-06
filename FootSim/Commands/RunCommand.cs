namespace FootSim.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using FootSim.Core;
    using FootSim.Grid;
    using FootSim.Options;

    public class RunCommand : ICommand
    {
        private readonly RunOptions options;

        public RunCommand(RunOptions options)
        {
            this.options = options;
        }

        public async Task<ExitCode> ExecuteAsync()
        {
            var repository = new Repository(this.options.Season.ForWeb(), this.options.League.ForWeb());

            Console.WriteLine($"League: {this.options.League.ForDisplay()}");
            Console.WriteLine($"Season: {this.options.Season.ForDisplay()}");
            Console.WriteLine();
            Console.WriteLine("League table:");
            Console.WriteLine();

            var seasonSoFar = new Season(repository.Matches(this.options.On));

            Console.WriteLine(CreateLeagueTable(seasonSoFar));

            Console.WriteLine();
            Console.WriteLine($"Simulating {this.options.Times:N0} times ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator();
            var result = seasonSimulator.Simulate(this.options.Times, seasonSoFar);

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(CreateSimulationTable(result.Teams));

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            return ExitCode.Success;
        }

        private static string CreateSimulationTable(IReadOnlyList<TeamSeasonSimulationResult> teams)
        {
            var gridBuilder = new GridBuilder<TeamSeasonSimulationResult>();

            gridBuilder.AddColumn("Name", Alignment.Left, tssr => tssr.TeamName);

            foreach (var position in Enumerable.Range(1, teams.Count))
            {
                gridBuilder.AddColumn($"#{position}", Alignment.Right, tssr => CalculatePercentage(position, tssr));
            }

            gridBuilder.AddColumn("Avg Pts", Alignment.Right, tssr => tssr.AveragePoints.ToString("N1"));

            return gridBuilder.Build(teams);
        }

        private static string CreateLeagueTable(Season season)
        {
            var gridBuilder = new GridBuilder<TablePlacing>();

            gridBuilder.AddColumn("#", Alignment.Right, tp => tp.Position);
            gridBuilder.AddColumn("Name", Alignment.Left, tp => tp.TeamName);
            gridBuilder.AddColumn("Pld", Alignment.Right, tp => tp.Played);
            gridBuilder.AddColumn("W", Alignment.Right, tp => tp.Won);
            gridBuilder.AddColumn("D", Alignment.Right, tp => tp.Drawn);
            gridBuilder.AddColumn("L", Alignment.Right, tp => tp.Lost);
            gridBuilder.AddColumn("GD", Alignment.Right, tp => tp.GoalDifference);
            gridBuilder.AddColumn("Pts", Alignment.Right, tp => tp.Points);

            return gridBuilder.Build(season.Table);
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