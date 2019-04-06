namespace FootSim.Options
{
    using CommandLine;
    using NodaTime;
    using NodaTime.Text;

    [Verb("run", HelpText = "Run the simulation of a season")]
    public class RunOptions
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.Iso;

        [Option('l', "league", Required = true,
            HelpText = "The league to run the simulation for. Supports \"epl\" (English Premier League), \"champ\" (English Championship), \"l1\" (English League One), \"l2\" (English League Two), \"conf\" (English Conference).")]
        public League League { get; set; }

        [Option('s', "season", Required = true, HelpText = "The season to run the simulation for. Denoted by the year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019.")]
        public int Season { get; set; }

        [Option('o', "on", Required = false, HelpText = "Date on which to perform the simulation. Format yyyy-MM-dd.")]
        public string OnString { get; set; }

        public LocalDate? On => string.IsNullOrEmpty(this.OnString) ? (LocalDate?)null : Pattern.Parse(this.OnString).GetValueOrThrow();

        [Option('t', "times", Required = false, Default = 10_000, HelpText = "The number of times to simulate the season.")]
        public int Times { get; set; }
    }
}