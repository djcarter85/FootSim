namespace FootballPredictor
{
    using System.Collections.Generic;

    public class Season
    {
        public Season(IReadOnlyList<PastMatch> matches)
        {
            this.Matches = matches;
        }

        public IReadOnlyList<PastMatch> Matches { get; }
    }
}