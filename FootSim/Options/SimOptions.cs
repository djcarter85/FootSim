namespace FootSim.Options
{
    using CommandLine;
    using NodaTime;
    using NodaTime.Text;

    [Verb("sim", HelpText = "Run the simulation of a season")]
    public class SimOptions
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.Iso;

        [Option('o', "on", Required = false, HelpText = "Date on which to perform the simulation. Format yyyy-MM-dd.")]
        public string OnString { get; set; }

        public LocalDate? On => string.IsNullOrEmpty(this.OnString) ? (LocalDate?)null : Pattern.Parse(this.OnString).GetValueOrThrow();

        [Option('t', "times", Required = false, Default = 10_000, HelpText = "The number of times to simulate the season.")]
        public int Times { get; set; }

        [Option('s', "season", Required = true, HelpText = "The season to run the simulation for, e.g. \"1819\".")]
        public string Season { get; set; }

        [Option('l', "league", Required = true, HelpText = "The league to run the simulation for. Supports \"epl\" (Premier League), \"champ\" (Championship).")]
        public League League { get; set; }
    }
}