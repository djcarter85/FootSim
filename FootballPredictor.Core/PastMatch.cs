namespace FootballPredictor.Core
{
    using Randomness.Distributions;
    using Randomness.Distributions.Discrete;

    public class PastMatch : IMatch
    {
        public PastMatch(string homeTeamName, string awayTeamName, Score score)
        {
            this.HomeTeamName = homeTeamName;
            this.AwayTeamName = awayTeamName;
            this.Score = score;
        }

        public string HomeTeamName { get; }

        public string AwayTeamName { get; }

        public Score Score { get; }

        public IDistribution<Score> ScoreDistribution => Singleton<Score>.Distribution(this.Score);
    }
}