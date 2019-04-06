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

        public Task<ExitCode> ExecuteAsync()
        {
            var seasonSoFar = TableCommand.CalculateAndDisplayLeagueTable(this.options.League, this.options.Season, this.options.On);

            Console.WriteLine();
            Console.WriteLine($"Simulating {this.options.Times:N0} times ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator();
            var result = seasonSimulator.Simulate(this.options.Times, seasonSoFar);

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(CreateSimulationGrid(result.Teams));

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            return Task.FromResult(ExitCode.Success);
        }

        private static string CreateSimulationGrid(IReadOnlyList<TeamSeasonSimulationResult> teams)
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

        private static string CalculatePercentage(int position, TeamSeasonSimulationResult teamSeasonSimulationResult)
        {
            var positionCount = teamSeasonSimulationResult.PositionCount(position);

            return positionCount == 0
                ? string.Empty
                : ((double)positionCount / teamSeasonSimulationResult.Positions.Count * 100).ToString(".0");
        }
    }
}