namespace FootballPredictor
{
    using System.Collections.Generic;

    public class MatchSimulationResult
    {
        public MatchSimulationResult(ExpectedScore expectedScore, IReadOnlyList<Score> sampleScores)
        {
            this.ExpectedScore = expectedScore;
            this.SampleScores = sampleScores;
        }

        public ExpectedScore ExpectedScore { get; }

        public IReadOnlyList<Score> SampleScores { get; }
    }
}