namespace FootSim.Options
{
    using CommandLine;
    using FootSim.Commands;
    using NodaTime;

    [Verb("match", HelpText = "Simulate a single match in a season.")]
    public class MatchOptions : IOptions
    {
        [Value(0, Required = true, HelpText = "The nation of the league. Supports \"ENG\" (England), \"FRA\" (France)\", \"GER\" (Germany), \"ITA\" (Italy), \"SPA\" (Spain).")]
        public NationOption Nation { get; set; }

        [Value(1, Required = true, HelpText = "The 0-based index of the league within the nation's football pyramid.")]
        public int Tier { get; set; }

        [Value(2, Required = false, HelpText = "The year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019. Defaults to the current season.")]
        public int? StartingYear { get; set; }

        [Option('h', "home", Required = true, HelpText = "The home team.")]
        public string Home { get; set; }

        [Option('a', "away", Required = true, HelpText = "The away team.")]
        public string Away { get; set; }

        [Option('t', "times", Required = false, Default = 10_000, HelpText = "The number of times to simulate the match.")]
        public int Times { get; set; }

        public ICommand CreateCommand()
        {
            return new MatchCommand(this, SystemClock.Instance);
        }
    }
}