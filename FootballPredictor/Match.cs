namespace FootballPredictor
{
    public class Match
    {
        public Match(string homeTeamName, string awayTeamName, int homeScore, int awayScore)
        {
            this.HomeTeamName = homeTeamName;
            this.AwayTeamName = awayTeamName;
            this.HomeScore = homeScore;
            this.AwayScore = awayScore;
        }

        public string HomeTeamName { get; }

        public string AwayTeamName { get; }

        public int HomeScore { get; }

        public int AwayScore { get; }
    }
}