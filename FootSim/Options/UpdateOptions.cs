namespace FootSim.Options
{
    using CommandLine;

    [Verb("update", HelpText = "Update the results from www.football-data.co.uk.")]
    public class UpdateOptions
    {
        [Option('s', "season", Required = true, HelpText = "The season to update the data for, e.g. \"1819\".")]
        public string Season { get; set; }

        [Option('l', "league", Required = true, HelpText = "The league to update the data for. Supports \"epl\" (Premier League), \"champ\" (Championship).")]
        public League League { get; set; }
    }
}