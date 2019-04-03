namespace FootballPredictor.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Randomness.Distributions;

    public static class SeasonSimulator
    {
        public static IReadOnlyDictionary<string, SeasonSimulationResult> Simulate(int simulations)
        {
            var repository = new Repository();

            var teams = Calculator.GetTeams(repository.TeamNames, repository.Matches);

            var distribution = SeasonDistribution.Create(
                repository.Matches,
                teams);

            var sampleSeasons = distribution
                .Samples()
                .Take(simulations)
                .ToArray();

            var results = new Dictionary<string, TempSimulationResult>();

            foreach (var sampleSeason in sampleSeasons)
            {
                foreach (var tablePlacing in sampleSeason.Table)
                {
                    if (!results.ContainsKey(tablePlacing.TeamName))
                    {
                        results[tablePlacing.TeamName] = new TempSimulationResult();
                    }

                    results[tablePlacing.TeamName].Positions.Add(tablePlacing.Position);
                    results[tablePlacing.TeamName].Points.Add(tablePlacing.Points);
                }
            }

            return results.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.SeasonSimulationResult);
        }

        private class TempSimulationResult
        {
            public List<int> Positions { get; } = new List<int>();

            public List<int> Points { get; } = new List<int>();

            public SeasonSimulationResult SeasonSimulationResult => new SeasonSimulationResult(this.Points, this.Positions);
        }
    }
}