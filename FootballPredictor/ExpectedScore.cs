namespace FootballPredictor
{
    public class ExpectedScore
    {
        public ExpectedScore(double expectedHomeGoals, double expectedAwayGoals)
        {
            this.ExpectedHomeGoals = expectedHomeGoals;
            this.ExpectedAwayGoals = expectedAwayGoals;
        }

        public double ExpectedHomeGoals { get; }

        public double ExpectedAwayGoals { get; }
    }
}