﻿namespace FootSim.Commands
{
    using System;
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

        public TableCommand(TableOptions options)
        {
            this.options = options;
        }

        public async Task<ExitCode> ExecuteAsync()
        {
            CalculateAndDisplayLeagueTable(this.options.League, this.options.Season, this.options.On);

            return ExitCode.Success;
        }

        public static Season CalculateAndDisplayLeagueTable(League league, int season, LocalDate? on)
        {
            var repository = new Repository(season.ForWeb(), league.ForWeb());

            Console.WriteLine($"League: {league.ForDisplay()}");
            Console.WriteLine($"Season: {season.ForDisplay()}");

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
            gridBuilder.AddColumn("GD", Alignment.Right, tp => tp.GoalDifference);
            gridBuilder.AddColumn("Pts", Alignment.Right, tp => tp.Points);

            return gridBuilder.Build(season.Table);
        }
    }
}