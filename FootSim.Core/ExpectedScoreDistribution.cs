namespace FootSim.Core
{
    using Randomness.Distributions;

    public class ExpectedScoreDistribution : IDistribution<Score>
    {
        private readonly ExpectedScore expectedScore;

        public ExpectedScoreDistribution(ExpectedScore expectedScore)
        {
            this.expectedScore = expectedScore;
        }

        public Score Sample()
        {
            var homeGoals = Poisson.Distribution(this.expectedScore.Home).Sample();
            var awayGoals = Poisson.Distribution(this.expectedScore.Away).Sample();

            return new Score(homeGoals, awayGoals);
        }
    }
}