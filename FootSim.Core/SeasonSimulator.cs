namespace FootSim.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Randomness.Distributions;

    public class SeasonSimulator
    {
        public SeasonSimulationResult Simulate(int simulations, Season seasonSoFar)
        {
            var teams = Calculator.GetTeams(seasonSoFar);

            var distribution = SeasonDistribution.Create(
                seasonSoFar,
                teams);

            var sampleSeasons = distribution
                .TakeSamples(simulations)
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
                .OrderBy(kvp => seasonSoFar.Table.Single(tp => tp.TeamName == kvp.Key).Position)
                .Select((kvp, i) => kvp.Value.TeamSeasonSimulationResult(i + 1, kvp.Key))
                .ToArray();

            return new SeasonSimulationResult(teamResults);
        }

        private class TempSimulationResult
        {
            public List<int> Positions { get; } = new List<int>();

            public List<int> Points { get; } = new List<int>();

            public TeamSeasonSimulationResult TeamSeasonSimulationResult(int currentPosition, string teamName) =>
                new TeamSeasonSimulationResult(currentPosition, teamName, this.Points, this.Positions);
        }
    }
}