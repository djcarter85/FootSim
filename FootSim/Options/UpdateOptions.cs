namespace FootSim.Options
{
    using CommandLine;

    [Verb("update", HelpText = "Update the results from www.football-data.co.uk.")]
    public class UpdateOptions
    {
        [Option('s', "season", Required = true, HelpText = "The season to update the data for. Denoted by the year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019.")]
        public int Season { get; set; }

        [Option('l', "league", Required = true, HelpText = "The league to update the data for. Supports \"epl\" (Premier League), \"champ\" (Championship).")]
        public League League { get; set; }
    }
}