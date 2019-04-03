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

        public IDistribution<Score> ScoreDistribution => new FutureScoreDistribution(this.expectedScore);

        private class FutureScoreDistribution : IDistribution<Score>
        {
            private readonly Poisson homeDistribution;
            private readonly Poisson awayDistribution;

            public FutureScoreDistribution(ExpectedScore expectedScore)
            {
                this.homeDistribution = Poisson.Distribution(expectedScore.Home);
                this.awayDistribution = Poisson.Distribution(expectedScore.Away);
            }

            public Score Sample()
            {
                return new Score(this.homeDistribution.Sample(), this.awayDistribution.Sample());
            }
        }
    }
}