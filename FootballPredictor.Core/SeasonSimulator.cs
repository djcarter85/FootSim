namespace FootballPredictor.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;
    using Randomness.Distributions;

    public class SeasonSimulator
    {
        private readonly Repository repository;

        public SeasonSimulator(Repository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyDictionary<string, SeasonSimulationResult> Simulate(int simulations, LocalDate? lastDate)
        {
            var matches = this.repository.Matches(lastDate);

            var teams = Calculator.GetTeams(this.repository.TeamNames, matches);

            var distribution = SeasonDistribution.Create(
                matches,
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