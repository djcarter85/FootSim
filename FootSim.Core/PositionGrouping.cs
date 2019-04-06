namespace FootSim.Core
{
    public class PositionGrouping
    {
        private readonly int minPosition;
        private readonly int maxPosition;

        public PositionGrouping(string longName, string shortName, int minPosition, int maxPosition)
        {
            this.LongName = longName;
            this.ShortName = shortName;
            this.minPosition = minPosition;
            this.maxPosition = maxPosition;
        }

        public string LongName { get; }

        public string ShortName { get; }

        public bool Contains(int position) => this.minPosition <= position && position <= this.maxPosition;
    }
}