namespace FootSim.Core
{
    using System.Collections.Generic;

    public class Score
    {
        private sealed class EqualityComparer : IEqualityComparer<Score>
        {
            public bool Equals(Score x, Score y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Home == y.Home && x.Away == y.Away;
            }

            public int GetHashCode(Score obj)
            {
                unchecked
                {
                    return (obj.Home * 397) ^ obj.Away;
                }
            }
        }

        public static IEqualityComparer<Score> Comparer { get; } = new EqualityComparer();

        public Score(int home, int away)
        {
            this.Home = home;
            this.Away = away;
        }

        public int Home { get; }

        public int Away { get; }

        public Result Result =>
            this.Home > this.Away ?
                Result.HomeWin :
                this.Home < this.Away ?
                    Result.AwayWin :
                    Result.Draw;

        public override string ToString()
        {
            return $"{this.Home}-{this.Away}";
        }
    }
}