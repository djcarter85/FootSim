namespace FootSim.Core
{
    public interface ICompletedMatch
    {
        string HomeTeamName { get; }

        string AwayTeamName { get; }

        Score Score { get; }
    }
}