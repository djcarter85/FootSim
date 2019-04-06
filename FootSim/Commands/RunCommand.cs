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
            if (this.options.Update)
            {
                await UpdateCommand.UpdateFromServer(this.options.League);
                Console.WriteLine();
            }

            var seasonSoFar = TableCommand.CalculateAndDisplayLeagueTable(
                this.options.League,
                this.options.On);

            Console.WriteLine();
            Console.WriteLine($"Simulating {this.options.Times:N0} times ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator();
            var result = seasonSimulator.Simulate(this.options.Times, seasonSoFar);

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(CreateSimulationGrid(result.Teams, this.options.League.PositionGroupings));

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            return ExitCode.Success;
        }

        private static string CreateSimulationGrid(
            IReadOnlyList<TeamSeasonSimulationResult> teams,
            IEnumerable<PositionGrouping> positionGroupings)
        {
            var gridBuilder = new GridBuilder<TeamSeasonSimulationResult>();

            gridBuilder.AddColumn("Name", Alignment.Left, tssr => tssr.TeamName);

            foreach (var position in Enumerable.Range(1, teams.Count))
            {
                gridBuilder.AddColumn($"#{position}", Alignment.Right, tssr => CalculatePercentage(position, tssr));
            }

            gridBuilder.AddColumn("Avg Pts", Alignment.Right, tssr => tssr.AveragePoints.ToString("N1"));

            foreach (var positionGrouping in positionGroupings)
            {
                gridBuilder.AddColumn(positionGrouping.Name, Alignment.Right, tssr => CalculatePercentage(positionGrouping, tssr));
            }

            return gridBuilder.Build(teams);
        }

        private static string CalculatePercentage(int position, TeamSeasonSimulationResult teamSeasonSimulationResult)
        {
            var count = teamSeasonSimulationResult.PositionCount(position);

            return count == 0
                ? string.Empty
                : ((double)count / teamSeasonSimulationResult.Positions.Count * 100).ToString(".0");
        }

        private static string CalculatePercentage(PositionGrouping positionGrouping, TeamSeasonSimulationResult teamSeasonSimulationResult)
        {
            var count = teamSeasonSimulationResult.PositionGroupingCount(positionGrouping);

            return count == 0
                ? string.Empty
                : ((double)count / teamSeasonSimulationResult.Positions.Count * 100).ToString(".0");
        }
    }
}