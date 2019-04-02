namespace FootballPredictor
{
    using System.Linq;
    using Randomness.Distributions;

    public static class MatchSimulator
    {
        public static MatchSimulationResult Simulate(string homeTeamName, string awayTeamName, int simulations)
        {
            var repository = new Repository();

            var teams = Calculator.GetTeams(repository.TeamNames, repository.Matches);

            var averageHomeGoals = Calculator.AverageHomeGoals(repository.Matches);
            var averageAwayGoals = Calculator.AverageAwayGoals(repository.Matches);

            var distribution = MatchDistribution.Create(
                homeTeamName,
                awayTeamName,
                teams,
                averageHomeGoals,
                averageAwayGoals);
            var expectedScore = distribution.ExpectedScore;

            var sampleScores = distribution.Samples().Take(simulations).ToArray();

            return new MatchSimulationResult(expectedScore, sampleScores);
        }
    }
}