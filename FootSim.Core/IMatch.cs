namespace FootSim.Core
{
    using Randomness.Distributions;

    public interface IMatch
    {
        string HomeTeamName { get; }

        string AwayTeamName { get; }

        IDistribution<Score> ScoreDistribution { get; }
    }
}