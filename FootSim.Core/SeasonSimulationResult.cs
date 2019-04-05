namespace FootSim.Core
{
    using System.Collections.Generic;

    public class SeasonSimulationResult
    {
        public SeasonSimulationResult(
            Season seasonSoFar,
            IReadOnlyList<TeamSeasonSimulationResult> teams)
        {
            this.SeasonSoFar = seasonSoFar;
            this.Teams = teams;
        }

        public Season SeasonSoFar { get; }

        public IReadOnlyList<TeamSeasonSimulationResult> Teams { get; }
    }
}