namespace FootSim.Core
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

        public SeasonSimulationResult Simulate(int simulations, Season seasonSoFar)
        {
            var teams = Calculator.GetTeams(this.repository.TeamNames, seasonSoFar);

            var distribution = SeasonDistribution.Create(
                seasonSoFar,
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

            var teamResults = results
                .Select(kvp => kvp.Value.TeamSeasonSimulationResult(kvp.Key))
                .ToArray();

            return new SeasonSimulationResult(teamResults);
        }

        private class TempSimulationResult
        {
            public List<int> Positions { get; } = new List<int>();

            public List<int> Points { get; } = new List<int>();

            public TeamSeasonSimulationResult TeamSeasonSimulationResult(string teamName) => new TeamSeasonSimulationResult(teamName, this.Points, this.Positions);
        }
    }
}