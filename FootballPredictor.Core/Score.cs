namespace FootballPredictor.Core
{
    public class Score
    {
        public Score(int home, int away)
        {
            this.Home = home;
            this.Away = away;
        }

        public int Home { get; }

        public int Away { get; }

        public Result Result =>
            this.Home > this.Away ?
                Result.HomeWin :
                this.Home < this.Away ?
                    Result.AwayWin :
                    Result.Draw;
    }
}