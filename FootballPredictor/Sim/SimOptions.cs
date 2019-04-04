namespace FootballPredictor.Sim
{
    using CommandLine;
    using NodaTime;
    using NodaTime.Text;

    [Verb("sim", HelpText = "Run the simulation of a season")]
    public class SimOptions
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.Iso;

        [Option('r', "refresh", Required = false, HelpText = "Refresh the results from www.football-data.co.uk.")]
        public bool Refresh { get; set; }

        [Option('u', "until", Required = false, HelpText = "Simulate the season up to and including matches played on the specified date. Format yyyy-MM-dd.")]
        public string UntilString { get; set; }

        public LocalDate? Until => string.IsNullOrEmpty(this.UntilString) ? (LocalDate?)null : Pattern.Parse(this.UntilString).GetValueOrThrow();

    }
}