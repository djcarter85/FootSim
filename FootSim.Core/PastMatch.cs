namespace FootSim.Core
{
    using NodaTime;

    public class PastMatch : ICompletedMatch
    {
        public PastMatch(LocalDate? date, string homeTeamName, string awayTeamName, Score score)
        {
            this.Date = date;
            this.HomeTeamName = homeTeamName;
            this.AwayTeamName = awayTeamName;
            this.Score = score;
        }

        public LocalDate? Date { get; }

        public string HomeTeamName { get; }

        public string AwayTeamName { get; }

        public Score Score { get; }
    }
}