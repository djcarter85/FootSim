namespace FootSim.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Randomness.Distributions;

    public class SeasonSimulator
    {
        public SeasonSimulationResult Simulate(bool resim, int simulations, Season seasonSoFar)
        {
            var distribution = SeasonDistribution.Create(resim, seasonSoFar);

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

                    results[tablePlacing.TeamName].GoalsFors.Add(tablePlacing.GoalsFor);
                    results[tablePlacing.TeamName].GoalsAgainsts.Add(tablePlacing.GoalsAgainst);
                    results[tablePlacing.TeamName].GoalDifferences.Add(tablePlacing.GoalDifference);
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
            public List<int> GoalsFors { get; } = new List<int>();

            public List<int> GoalsAgainsts { get; } = new List<int>();

            public List<int> GoalDifferences { get; } = new List<int>();

            public List<int> Positions { get; } = new List<int>();

            public List<int> Points { get; } = new List<int>();

            public TeamSeasonSimulationResult TeamSeasonSimulationResult(int currentPosition, string teamName) =>
                new TeamSeasonSimulationResult(
                    currentPosition, teamName,
                    this.GoalsFors,
                    this.GoalsAgainsts,
                    this.GoalDifferences,
                    this.Points,
                    this.Positions);
        }
    }
}