namespace FootSim.Options
{
    using CommandLine;
    using FootSim.Commands;
    using FootSim.Core;
    using NodaTime;
    using NodaTime.Text;

    [Verb("table", HelpText = "Display the league table for a season.")]
    public class TableOptions : IOptions
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.Iso;

        [Value(0, Required = true, HelpText = "The nation of the league. Supports \"ENG\" (England).")]
        public NationOption Nation { get; set; }

        [Value(1, Required = true, HelpText = "The 0-based index of the league within the nation's football pyramid.")]
        public int Tier { get; set; }

        [Value(2, Required = true, HelpText = "The year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019.")]
        public int StartingYear { get; set; }

        public League League => new League(this.Nation.ToNation(), this.Tier, this.StartingYear);

        [Option('o', "on", Required = false, HelpText = "Date on which to perform the simulation. Format yyyy-MM-dd.")]
        public string OnString { get; set; }

        public LocalDate? On => string.IsNullOrEmpty(this.OnString) ? (LocalDate?)null : Pattern.Parse(this.OnString).GetValueOrThrow();

        public ICommand CreateCommand() => new TableCommand(this);
    }
}