namespace FootSim.Options
{
    using CommandLine;
    using FootSim.Commands;
    using NodaTime;

    [Verb("update", HelpText = "Update the results from www.football-data.co.uk.")]
    public class UpdateOptions : IOptions
    {
        [Value(0, Required = true, HelpText = "The nation of the league. Supports \"ENG\" (England), \"FRA\" (France)\", \"GER\" (Germany), \"ITA\" (Italy), \"SPA\" (Spain).")]
        public NationOption Nation { get; set; }

        [Value(1, Required = true, HelpText = "The 0-based index of the league within the nation's football pyramid.")]
        public int Tier { get; set; }

        [Value(2, Required = false, HelpText = "The year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019. Defaults to the current season.")]
        public int? StartingYear { get; set; }

        public ICommand CreateCommand() => new UpdateCommand(this, SystemClock.Instance);
    }
}