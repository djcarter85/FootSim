namespace FootSim.Options
{
    using CommandLine;
    using FootSim.Commands;
    using NodaTime;

    [Verb("run", HelpText = "Run the simulation of a season.")]
    public class RunOptions : IOptions
    {
        [Value(0, Required = true, HelpText = "The nation of the league. Supports \"ENG\" (England), \"FRA\" (France)\", \"GER\" (Germany), \"ITA\" (Italy), \"SPA\" (Spain).")]
        public NationOption Nation { get; set; }

        [Value(1, Required = true, HelpText = "The 0-based index of the league within the nation's football pyramid.")]
        public int Tier { get; set; }

        [Value(2, Required = false, HelpText = "The year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019. Defaults to the current season.")]
        public int? StartingYear { get; set; }

        [Option('o', "on", Required = false, HelpText = "Date on which to perform the simulation. Format yyyy-MM-dd.")]
        public string On { get; set; }

        [Option('t', "times", Required = false, Default = 10_000, HelpText = "The number of times to simulate the season.")]
        public int Times { get; set; }

        [Option('u', "update", Required = false, HelpText = "Whether to update the results from the server first.")]
        public bool Update { get; set; }

        [Option('c', "csv", Required = false, HelpText = "If specified, the file path to output the results to in CSV format.")]
        public string Csv { get; set; }

        public ICommand CreateCommand() => new RunCommand(this, SystemClock.Instance);
    }
}