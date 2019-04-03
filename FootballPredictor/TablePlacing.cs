namespace FootballPredictor
{
    public class TablePlacing
    {
        public TablePlacing(int position, string teamName, int won, int drawn, int lost, int goalsFor, int goalsAgainst, int goalDifference, int points)
        {
            this.Position = position;
            this.TeamName = teamName;
            this.Won = won;
            this.Drawn = drawn;
            this.Lost = lost;
            this.GoalsFor = goalsFor;
            this.GoalsAgainst = goalsAgainst;
            this.GoalDifference = goalDifference;
            this.Points = points;
        }

        public int Position { get; }

        public string TeamName { get; }

        public int Won { get; }

        public int Drawn { get; }

        public int Lost { get; }

        public int GoalsFor { get; }

        public int GoalsAgainst { get; }

        public int GoalDifference { get; }

        public int Points { get; }
    }
}