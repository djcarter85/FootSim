namespace FootSim.Core
{
    using System.Collections.Generic;

    public class SeasonSimulationResult
    {
        public SeasonSimulationResult(IReadOnlyList<TeamSeasonSimulationResult> teams)
        {
            this.Teams = teams;
        }

        public IReadOnlyList<TeamSeasonSimulationResult> Teams { get; }
    }
}