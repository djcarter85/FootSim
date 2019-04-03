namespace FootballPredictor
{
    using System.Collections.Generic;
    using System.Linq;
    using Randomness.Distributions;

    public static class SeasonSimulator
    {
        public static IReadOnlyList<Season> Simulate(int simulations)
        {
            var distribution = CreateSeasonDistribution();

            return distribution
                .Samples()
                .Take(simulations)
                .ToArray();
        }

        public static SeasonDistribution CreateSeasonDistribution()
        {
            var repository = new Repository();

            var teams = Calculator.GetTeams(repository.TeamNames, repository.Matches);

            return SeasonDistribution.Create(
                repository.Matches,
                teams);
        }
    }
}