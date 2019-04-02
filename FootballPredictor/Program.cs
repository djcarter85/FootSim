namespace FootballPredictor
{
    using System;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var homeTeamName = "Man City";
            var awayTeamName = "Huddersfield";
            var simulations = 20;

            var simulationResult = MatchSimulator.Simulate(homeTeamName, awayTeamName, simulations);

            Console.WriteLine($"{homeTeamName} v {awayTeamName}");
            Console.WriteLine($"Expected: {simulationResult.ExpectedScore.ExpectedHomeGoals:N2}-{simulationResult.ExpectedScore.ExpectedAwayGoals:N2}");
            Console.WriteLine();

            Console.WriteLine("Simulated:");
            foreach (var score in simulationResult.SampleScores)
            {
                Console.WriteLine(GetScoreDescription(score));
            }

            Console.ReadLine();
        }

        private static string GetScoreDescription(Score score)
        {
            return $"{GetResultDescription(score.Result)} {score.HomeScore}-{score.AwayScore}";
        }

        private static string GetResultDescription(Result result)
        {
            switch (result)
            {
                case Result.HomeWin:
                    return "***    ";
                case Result.Draw:
                    return " *   * ";
                case Result.AwayWin:
                    return "    ***";
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    }
}
