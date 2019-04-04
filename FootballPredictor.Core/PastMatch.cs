namespace FootballPredictor.Core
{
    using NodaTime;
    using Randomness.Distributions;
    using Randomness.Distributions.Discrete;

    public class PastMatch : IMatch
    {
        public PastMatch(LocalDate? date, string homeTeamName, string awayTeamName, Score score)
        {
            this.Date = date;
            this.HomeTeamName = homeTeamName;
            this.AwayTeamName = awayTeamName;
            this.Score = score;
        }

        public LocalDate? Date { get; }

        public string HomeTeamName { get; }

        public string AwayTeamName { get; }

        public Score Score { get; }

        public IDistribution<Score> ScoreDistribution => Singleton<Score>.Distribution(this.Score);
    }
}