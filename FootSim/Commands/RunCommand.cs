namespace FootSim.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CsvHelper;
    using FootSim.Core;
    using FootSim.Grid;
    using FootSim.Options;
    using NodaTime;

    public class RunCommand : ICommand
    {
        private readonly RunOptions options;
        private readonly IClock clock;

        public RunCommand(RunOptions options, IClock clock)
        {
            this.options = options;
            this.clock = clock;
        }

        public async Task<ExitCode> ExecuteAsync()
        {
            var league = new League(
                Conversions.ToNation(this.options.Nation),
                this.options.Tier,
                Conversions.ToStartingYear(this.options.StartingYear, this.clock));

            if (this.options.Update)
            {
                await UpdateCommand.UpdateFromServer(league);
                Console.WriteLine();
            }

            var seasonSoFar = TableCommand.CalculateAndDisplayLeagueTable(league, this.options.On);

            Console.WriteLine();
            Console.WriteLine($"Simulating {this.options.Times:N0} times ...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var seasonSimulator = new SeasonSimulator();
            var result = seasonSimulator.Simulate(this.options.Times, seasonSoFar);

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(CreateSimulationGrid(result.Teams, league.PositionGroupings));

            Console.WriteLine();
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");

            if (this.options.Csv != null)
            {
                await OutputToCsv(result.Teams, league.PositionGroupings, this.options.Csv);

                Console.WriteLine();
                Console.WriteLine($"Results output to {this.options.Csv}");
            }

            return ExitCode.Success;
        }

        private static async Task OutputToCsv(
            IReadOnlyList<TeamSeasonSimulationResult> teams,
            IEnumerable<PositionGrouping> positionGroupings,
            string csvFilePath)
        {
            var gridBuilder = CreateSimulationGridBuilder(teams, positionGroupings);

            using (var streamWriter = new StreamWriter(csvFilePath))
            {
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    await gridBuilder.WriteToCsv(csvWriter, teams);
                }
            }
        }

        private static string CreateSimulationGrid(
            IReadOnlyList<TeamSeasonSimulationResult> teams,
            IEnumerable<PositionGrouping> positionGroupings)
        {
            var gridBuilder = CreateSimulationGridBuilder(teams, positionGroupings);

            return gridBuilder.Build(teams);
        }

        private static GridBuilder<TeamSeasonSimulationResult> CreateSimulationGridBuilder(IReadOnlyList<TeamSeasonSimulationResult> teams, IEnumerable<PositionGrouping> positionGroupings)
        {
            var gridBuilder = new GridBuilder<TeamSeasonSimulationResult>();

            gridBuilder.AddColumn("#", Alignment.Right, tssr => tssr.CurrentPosition);
            gridBuilder.AddColumn("Name", Alignment.Left, tssr => tssr.TeamName);

            foreach (var position in Enumerable.Range(1, teams.Count))
            {
                gridBuilder.AddColumn($"#{position}", Alignment.Right, tssr => CalculatePercentage(position, tssr));
            }

            gridBuilder.AddColumn("Avg Pts", Alignment.Right, tssr => tssr.AveragePoints.ToString("N1"));

            foreach (var positionGrouping in positionGroupings)
            {
                gridBuilder.AddColumn(positionGrouping.ShortName, Alignment.Right,
                    tssr => CalculatePercentage(positionGrouping, tssr));
            }

            gridBuilder.AddColumn("Name", Alignment.Left, tssr => tssr.TeamName);
            return gridBuilder;
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