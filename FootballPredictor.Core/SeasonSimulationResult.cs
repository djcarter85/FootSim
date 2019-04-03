namespace FootballPredictor.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public class SeasonSimulationResult
    {
        public SeasonSimulationResult(IReadOnlyList<int> points, IReadOnlyList<int> positions)
        {
            this.Points = points;
            this.Positions = positions;
        }

        public IReadOnlyList<int> Points { get; }

        public IReadOnlyList<int> Positions { get; }

        public double AveragePoints => this.Points.Average();

        public double PositionPercentage(int position) =>
            (double) this.Positions.Count(p => p == position) / this.Positions.Count;
    }
}