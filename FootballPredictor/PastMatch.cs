namespace FootballPredictor
{
    public class PastMatch
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
    }
}