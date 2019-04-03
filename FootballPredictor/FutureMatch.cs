namespace FootballPredictor
{
    using Randomness.Distributions;
    using Randomness.Distributions.Discrete;

    public class FutureMatch : IMatch
    {
        private readonly ExpectedScore expectedScore;

        public FutureMatch(string homeTeamName, string awayTeamName, ExpectedScore expectedScore)
        {
            this.HomeTeamName = homeTeamName;
            this.AwayTeamName = awayTeamName;
            this.expectedScore = expectedScore;
        }

        public string HomeTeamName { get; }

        public string AwayTeamName { get; }

        public IDistribution<Score> ScoreDistribution =>
            from homeGoals in Poisson.Distribution(this.expectedScore.Home)
            from awayGoals in Poisson.Distribution(this.expectedScore.Away)
            select new Score(homeGoals, awayGoals);
    }
}