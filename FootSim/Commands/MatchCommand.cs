namespace FootSim.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FootSim.Core;
    using FootSim.Options;
    using NodaTime;
    using Randomness.Distributions;

    public class MatchCommand : ICommand
    {
        private readonly MatchOptions options;
        private readonly IClock clock;

        private readonly IPointsCalculator pointsCalculator = new WcpPointsCalculator();

        public MatchCommand(MatchOptions options, IClock clock)
        {
            this.options = options;
            this.clock = clock;
        }

        public class ScoreFrequency
        {
            public Score Score { get; }
            public Percentage Percentage { get; }

            public ScoreFrequency(Score score, Percentage percentage)
            {
                this.Score = score;
                this.Percentage = percentage;
            }
        }

        public class ScoreExpectedPoints
        {
            public Score Score { get; }
            public decimal ExpectedPoints { get; }

            public ScoreExpectedPoints(Score score, decimal expectedPoints)
            {
                this.Score = score;
                this.ExpectedPoints = expectedPoints;
            }
        }

        public Task<ExitCode> ExecuteAsync()
        {
            var league = new League(
                Conversions.ToNation(this.options.Nation),
                this.options.Tier,
                Conversions.ToStartingYear(this.options.StartingYear, this.clock));

            var season = TableCommand.CalculateAndDisplayLeagueTable(league, on: null);
            Console.WriteLine();

            var homeConstant = Calculator.CalculateHomeConstant(season);
            var awayConstant = Calculator.CalculateAwayConstant(season);

            // TODO: cope with null
            var homeTablePlacing = season.Table.SingleOrDefault(tp => tp.TeamName == this.options.Home);
            var awayTablePlacing = season.Table.SingleOrDefault(tp => tp.TeamName == this.options.Away);

            var expectedScore = Calculator.CalculateExpectedScore(homeTablePlacing, awayTablePlacing, homeConstant, awayConstant);

            Console.WriteLine($"{homeTablePlacing.TeamName} v {awayTablePlacing.TeamName}");
            Console.WriteLine($"Expected: {expectedScore.Home:N2} - {expectedScore.Away:N2}");
            Console.WriteLine();

            var distribution = new ExpectedScoreDistribution(expectedScore);

            var sampleScores = distribution
                .TakeSamples(this.options.Times)
                .ToList();

            var probabilities = CalculateProbabilities(sampleScores);

            Console.WriteLine($"{homeTablePlacing.TeamName}: {probabilities[Result.HomeWin]}");
            Console.WriteLine($"Draw: {probabilities[Result.Draw]}");
            Console.WriteLine($"{awayTablePlacing.TeamName}: {probabilities[Result.AwayWin]}");
            Console.WriteLine();

            var scoreFrequencies = CalculateScoreFrequencies(sampleScores);

            var expectedPoints = scoreFrequencies
                .Select(sf => sf.Score)
                .Select(s => new ScoreExpectedPoints(s, this.CalculateExpectedPoints(s, scoreFrequencies)))
                .OrderByDescending(sep => sep.ExpectedPoints)
                .ToList();

            foreach (var scoreFrequency in scoreFrequencies.Take(5))
            {
                Console.WriteLine($"{scoreFrequency.Score}: {scoreFrequency.Percentage}");
            }

            Console.WriteLine();

            foreach (var exp in expectedPoints.Take(5))
            {
                Console.WriteLine($"{exp.Score}: {exp.ExpectedPoints:N2}");
            }

            return Task.FromResult(ExitCode.Success);
        }

        private static IReadOnlyList<ScoreFrequency> CalculateScoreFrequencies(IReadOnlyList<Score> sampleScores)
        {
            return sampleScores
                .GroupBy(s => s, Score.Comparer)
                .Select(g => new ScoreFrequency(g.Key, Percentage.FromFraction(g.Count(), sampleScores.Count)))
                .OrderByDescending(sf => sf.Percentage)
                .ThenBy(sf => sf.Score.Home)
                .ThenBy(sf => sf.Score.Away)
                .ToList();
        }

        private static IReadOnlyDictionary<Result, Percentage> CalculateProbabilities(IEnumerable<Score> sampleScores)
        {
            var dict = new Dictionary<Result, int> { { Result.HomeWin, 0 }, { Result.Draw, 0 }, { Result.AwayWin, 0 } };

            var count = 0;
            foreach (var sampleScore in sampleScores)
            {
                dict[sampleScore.Result]++;
                count++;
            }

            return dict.ToDictionary(kvp => kvp.Key, kvp => Percentage.FromFraction(kvp.Value, count));
        }

        private decimal CalculateExpectedPoints(Score predictedScore, IEnumerable<ScoreFrequency> scoreFrequencies)
        {
            return scoreFrequencies
                .Sum(sf => sf.Percentage.Proportion * this.pointsCalculator.CalculatePoints(predictedScore, sf.Score));
        }
    }
}