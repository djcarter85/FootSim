namespace FootballPredictor
{
    public class TablePlacing
    {
        public TablePlacing(string teamName, int won, int drawn, int lost, int goalsFor, int goalsAgainst)
        {
            this.TeamName = teamName;
            this.Won = won;
            this.Drawn = drawn;
            this.Lost = lost;
            this.GoalsFor = goalsFor;
            this.GoalsAgainst = goalsAgainst;
        }

        public string TeamName { get; }

        public int Won { get; }

        public int Drawn { get; }

        public int Lost { get; }

        public int GoalsFor { get; }

        public int GoalsAgainst { get; }

        public int GoalDifference => this.GoalsFor - this.GoalsAgainst;

        public int Points => 3 * this.Won + this.Drawn;
    }
}