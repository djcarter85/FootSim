namespace FootSim.Options
{
    using CommandLine;

    [Verb("update", HelpText = "Update the results from www.football-data.co.uk.")]
    public class UpdateOptions
    {
        [Option('s', "season", Required = true, HelpText = "The season to run the simulation for, e.g. \"1819\".")]
        public string Season { get; set; }
    }
}