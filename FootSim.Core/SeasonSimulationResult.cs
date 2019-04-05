namespace FootSim.Core
{
    using System.Collections.Generic;

    public class SeasonSimulationResult
    {
        public SeasonSimulationResult(IReadOnlyDictionary<string, TeamSeasonSimulationResult> teams)
        {
            this.Teams = teams;
        }

        public IReadOnlyDictionary<string, TeamSeasonSimulationResult> Teams { get; }
    }
}