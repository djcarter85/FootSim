namespace FootSim.Core
{
    public class TablePlacing
    {
        public TablePlacing(int position, string teamName, int played, int won, int drawn, int lost, int goalsFor, int goalsAgainst, int goalDifference, int points, double attackingStrength, double defensiveWeakness)
        {
            this.Position = position;
            this.TeamName = teamName;
            this.Played = played;
            this.Won = won;
            this.Drawn = drawn;
            this.Lost = lost;
            this.GoalsFor = goalsFor;
            this.GoalsAgainst = goalsAgainst;
            this.GoalDifference = goalDifference;
            this.Points = points;
            this.AttackingStrength = attackingStrength;
            this.DefensiveWeakness = defensiveWeakness;
        }

        public int Position { get; }

        public string TeamName { get; }

        public int Played { get; }

        public int Won { get; }

        public int Drawn { get; }

        public int Lost { get; }

        public int GoalsFor { get; }

        public int GoalsAgainst { get; }

        public int GoalDifference { get; }

        public int Points { get; }

        public double AttackingStrength { get; }

        public double DefensiveWeakness { get; }
    }
}