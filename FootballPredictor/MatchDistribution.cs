namespace FootballPredictor
{
    using System.Collections.Generic;
    using Randomness.Distributions;
    using Randomness.Distributions.Discrete;

    public class MatchDistribution : IDistribution<Score>
    {
        public MatchDistribution(ExpectedScore expectedScore)
        {
            this.ExpectedScore = expectedScore;
        }

        public ExpectedScore ExpectedScore { get; }

        public static MatchDistribution Create(
            string homeTeamName,
            string awayTeamName,
            IReadOnlyDictionary<string, Team> teams,
            double averageHomeGoals,
            double averageAwayGoals)
        {
            var expectedScore = Calculator.CalculateExpectedScore(
                homeTeamName,
                awayTeamName,
                teams,
                averageHomeGoals,
                averageAwayGoals);

            return new MatchDistribution(expectedScore);
        }

        public Score Sample()
        {
            var simulatedHome = Poisson.Distribution(this.ExpectedScore.ExpectedHomeGoals).Sample();
            var simulatedAway = Poisson.Distribution(this.ExpectedScore.ExpectedAwayGoals).Sample();

            return new Score(simulatedHome, simulatedAway);
        }
    }
}