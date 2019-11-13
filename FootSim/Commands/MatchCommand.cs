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

            var scoreFrequencies = sampleScores
                .GroupBy(s => s, Score.Comparer)
                .Select(g => new ScoreFrequency(g.Key, Percentage.FromFraction(g.Count(), sampleScores.Count)))
                .OrderByDescending(sf => sf.Percentage)
                .ThenBy(sf => sf.Score.Home)
                .ThenBy(sf => sf.Score.Away);

            foreach (var scoreFrequency in scoreFrequencies)
            {
                Console.WriteLine($"{scoreFrequency.Score.Home}-{scoreFrequency.Score.Away}: {scoreFrequency.Percentage}");
            }

            return Task.FromResult(ExitCode.Success);
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
    }
}