namespace FootSim.Core
{
    public class SimulatedMatch
    {
        public SimulatedMatch(string homeTeamName, string awayTeamName, Score score)
        {
            this.HomeTeamName = homeTeamName;
            this.AwayTeamName = awayTeamName;
            this.Score = score;
        }

        public string HomeTeamName { get; }

        public string AwayTeamName { get; }

        public Score Score { get; }
    }
}