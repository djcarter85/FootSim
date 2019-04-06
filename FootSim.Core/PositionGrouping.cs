namespace FootSim.Core
{
    using System.Collections.Generic;

    public class PositionGrouping
    {
        public PositionGrouping(string name, params int[] positions)
        {
            this.Name = name;
            this.Positions = positions;
        }

        public string Name { get; }

        public IReadOnlyList<int> Positions { get; }
    }
}