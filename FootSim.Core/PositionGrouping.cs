namespace FootSim.Core
{
    using System.Collections.Generic;

    public class PositionGrouping
    {
        public PositionGrouping(string longName, string shortName, params int[] positions)
        {
            this.LongName = longName;
            this.ShortName = shortName;
            this.Positions = positions;
        }

        public string LongName { get; }

        public string ShortName { get; }

        public IReadOnlyList<int> Positions { get; }
    }
}