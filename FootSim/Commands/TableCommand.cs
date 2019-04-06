namespace FootSim.Commands
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

        public TableCommand(TableOptions options)
        {
            this.options = options;
        }

        public Task<ExitCode> ExecuteAsync()
        {
            CalculateAndDisplayLeagueTable(this.options.League, this.options.On);

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
            gridBuilder.AddColumn("GD", Alignment.Right, tp => tp.GoalDifference);
            gridBuilder.AddColumn("Pts", Alignment.Right, tp => tp.Points);

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