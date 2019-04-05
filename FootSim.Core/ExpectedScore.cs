namespace FootSim.Core
{
    public class ExpectedScore
    {
        public ExpectedScore(double home, double away)
        {
            this.Home = home;
            this.Away = away;
        }

        public double Home { get; }

        public double Away { get; }
    }
}