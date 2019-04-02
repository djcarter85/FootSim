namespace FootballPredictor
{
    public class Score
    {
        public Score(int homeScore, int awayScore)
        {
            this.HomeScore = homeScore;
            this.AwayScore = awayScore;
        }

        public int HomeScore { get; }

        public int AwayScore { get; }

        public Result Result =>
            this.HomeScore > this.AwayScore ?
                Result.HomeWin :
                this.HomeScore < this.AwayScore ?
                    Result.AwayWin :
                    Result.Draw;
    }
}