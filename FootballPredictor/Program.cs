namespace FootballPredictor
{
    using System;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var homeTeamName = "Wolves";
            var awayTeamName = "Man United";
            var simulations = 20;

            var simulationResult = MatchSimulator.Simulate(homeTeamName, awayTeamName, simulations);

            Console.WriteLine($"{homeTeamName} v {awayTeamName}");
            Console.WriteLine($"Expected: {simulationResult.ExpectedScore.Home:N2}-{simulationResult.ExpectedScore.Away:N2}");
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
            return $"{GetResultDescription(score.Result)} {score.Home}-{score.Away}";
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
