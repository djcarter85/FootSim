namespace FootSim.Core
{
    using Randomness.Distributions;

    public interface ISimulatableMatch
    {
        string HomeTeamName { get; }

        string AwayTeamName { get; }

        IDistribution<Score> ScoreDistribution { get; }
    }
}