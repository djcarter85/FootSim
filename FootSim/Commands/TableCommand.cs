﻿namespace FootSim.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FootSim.Core;
    using FootSim.Grid;
    using FootSim.Options;
    using NodaTime;
    using NodaTime.Text;

    public class TableCommand : ICommand
    {
        private static readonly IPattern<LocalDate> LocalDatePattern =
            NodaTime.Text.LocalDatePattern.CreateWithCurrentCulture("dd MMM yyyy");

        private readonly TableOptions options;
        private readonly IClock clock;

        public TableCommand(TableOptions options, IClock clock)
        {
            this.options = options;
            this.clock = clock;
        }

        public Task<ExitCode> ExecuteAsync()
        {
            var league = new League(
                Conversions.ToNation(this.options.Nation),
                this.options.Tier,
                Conversions.ToStartingYear(this.options.StartingYear, this.clock));

            CalculateAndDisplayLeagueTable(league, Conversions.ToDate(this.options.On));

            return Task.FromResult(ExitCode.Success);
        }

        public static Season CalculateAndDisplayLeagueTable(League league, LocalDate? on)
        {
            var repository = new Repository(league);

            Console.WriteLine($"League: {league.Description}");
            Console.WriteLine($"Edition: {league.EditionDescription}");

            if (on != null)
            {
                Console.WriteLine($"On: {LocalDatePattern.Format(on.Value)}");
            }

            Console.WriteLine();

            var seasonSoFar = repository.Season(on);

            Console.WriteLine(CreateLeagueTableGrid(seasonSoFar));

            return seasonSoFar;
        }

        private static string CreateLeagueTableGrid(Season season)
        {
            var gridBuilder = new GridBuilder<TablePlacing>();

            gridBuilder.AddColumn("#", Alignment.Right, tp => tp.Position);
            gridBuilder.AddColumn("Name", Alignment.Left, tp => tp.TeamName);
            gridBuilder.AddColumn("Pld", Alignment.Right, tp => tp.Played);
            gridBuilder.AddColumn("W", Alignment.Right, tp => tp.Won);
            gridBuilder.AddColumn("D", Alignment.Right, tp => tp.Drawn);
            gridBuilder.AddColumn("L", Alignment.Right, tp => tp.Lost);
            gridBuilder.AddColumn("GF", Alignment.Right, tp => tp.GoalsFor);
            gridBuilder.AddColumn("GA", Alignment.Right, tp => tp.GoalsAgainst);
            gridBuilder.AddColumn("GD", Alignment.Right, tp => tp.GoalDifference);
            gridBuilder.AddColumn("Pts", Alignment.Right, tp => tp.Points);
            gridBuilder.AddColumn("AS", Alignment.Right, tp => tp.AttackingStrength.ToString("N2"));
            gridBuilder.AddColumn("DW", Alignment.Right, tp => tp.DefensiveWeakness.ToString("N2"));

            gridBuilder.AddColumn(
                string.Empty,
                Alignment.Left,
                tp => season.League.PositionGroupings
                    .Where(pg => pg.Contains(tp.Position))
                    .Select(pg => pg.LongName)
                    .Join(", "));

            return gridBuilder.Build(season.Table);
        }
    }
}