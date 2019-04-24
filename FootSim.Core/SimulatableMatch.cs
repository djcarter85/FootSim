namespace FootSim.Core
{
    using Randomness.Distributions;

    public class SimulatableMatch : ISimulatableMatch
    {
        private SimulatableMatch(string homeTeamName, string awayTeamName, IDistribution<Score> scoreDistribution)
        {
            this.HomeTeamName = homeTeamName;
            this.AwayTeamName = awayTeamName;
            this.ScoreDistribution = scoreDistribution;
        }

        public string HomeTeamName { get; }

        public string AwayTeamName { get; }

        public IDistribution<Score> ScoreDistribution { get; }

        public static ISimulatableMatch CreateFromCompletedMatch(ICompletedMatch completedMatch)
        {
            return new SimulatableMatch(
                completedMatch.HomeTeamName,
                completedMatch.AwayTeamName,
                Singleton.Distribution(completedMatch.Score));
        }

        public static ISimulatableMatch CreateFromExpectedScore(string homeTeamName, string awayTeamName, ExpectedScore expectedScore)
        {
            return new SimulatableMatch(homeTeamName, awayTeamName, new ExpectedScoreDistribution(expectedScore));
        }
    }
}