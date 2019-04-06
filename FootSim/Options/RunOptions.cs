namespace FootSim.Options
{
    using CommandLine;
    using FootSim.Commands;
    using FootSim.Core;
    using NodaTime;
    using NodaTime.Text;

    [Verb("run", HelpText = "Run the simulation of a season")]
    public class RunOptions : IOptions
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.Iso;

        [Value(0, Required = true, HelpText = "The nation of the league. Supports \"ENG\" (England).")]
        public NationOption Nation { get; set; }

        [Value(1, Required = true, HelpText = "The 0-based index of the league within the nation's football pyramid.")]
        public int Tier { get; set; }

        [Value(2, Required = true, HelpText = "The year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019.")]
        public int StartingYear { get; set; }

        public League League => new League(Conversions.ToNation(this.Nation), this.Tier, Conversions.ToStartingYear(this.StartingYear));

        [Option('o', "on", Required = false, HelpText = "Date on which to perform the simulation. Format yyyy-MM-dd.")]
        public string OnString { get; set; }

        public LocalDate? On => string.IsNullOrEmpty(this.OnString) ? (LocalDate?)null : Pattern.Parse(this.OnString).GetValueOrThrow();

        [Option('t', "times", Required = false, Default = 10_000, HelpText = "The number of times to simulate the season.")]
        public int Times { get; set; }

        [Option('u', "update", Required = false, Default = false, HelpText = "Whether to update the results from the server first.")]
        public bool Update { get; set; }

        [Option('c', "csv", Required = false, HelpText = "A CSV file to output the results to.")]
        public string Csv { get; set; }

        public ICommand CreateCommand() => new RunCommand(this);
    }
}