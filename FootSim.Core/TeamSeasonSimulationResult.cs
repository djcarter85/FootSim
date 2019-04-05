namespace FootSim.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public class TeamSeasonSimulationResult
    {
        public TeamSeasonSimulationResult(string teamName, IReadOnlyList<int> points, IReadOnlyList<int> positions)
        {
            this.TeamName = teamName;
            this.Points = points;
            this.Positions = positions;
        }

        public string TeamName { get; }

        public IReadOnlyList<int> Points { get; }

        public IReadOnlyList<int> Positions { get; }

        public double AveragePoints => this.Points.Average();

        public int PositionCount(int position) => this.Positions.Count(p => p == position);
    }
}