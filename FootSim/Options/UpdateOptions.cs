namespace FootSim.Options
{
    using CommandLine;
    using FootSim.Commands;
    using FootSim.Core;

    [Verb("update", HelpText = "Update the results from www.football-data.co.uk.")]
    public class UpdateOptions : IOptions
    {
        [Value(0, Required = true, HelpText = "The nation of the league. Supports \"ENG\" (England).")]
        public NationOption Nation { get; set; }

        [Value(1, Required = true, HelpText = "The 0-based index of the league within the nation's football pyramid.")]
        public int Tier { get; set; }

        [Value(2, Required = true, HelpText = "The year in which the season starts, e.g. \"2018\" or \"18\" for 2018-2019.")]
        public int StartingYear { get; set; }

        public League League => new League(this.Nation.ToNation(), this.Tier, this.StartingYear);

        public ICommand CreateCommand() => new UpdateCommand(this);
    }
}