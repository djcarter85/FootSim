namespace FootballPredictor
{
    public class Match
    {
        public Match(string homeTeam, string awayTeam, int homeScore, int awayScore)
        {
            this.HomeTeam = homeTeam;
            this.AwayTeam = awayTeam;
            this.HomeScore = homeScore;
            this.AwayScore = awayScore;
        }

        public string HomeTeam { get; }

        public string AwayTeam { get; }

        public int HomeScore { get; }

        public int AwayScore { get; }
    }
}