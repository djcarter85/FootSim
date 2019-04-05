namespace FootballPredictor.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Randomness.Distributions;

    public class SeasonDistribution : IDistribution<Season>
    {
        private readonly IReadOnlyList<IMatch> matches;

        private SeasonDistribution(IReadOnlyList<IMatch> matches)
        {
            this.matches = matches;
        }

        public static SeasonDistribution Create(IReadOnlyList<PastMatch> pastMatches, IReadOnlyList<Team> teams)
        {
            return new SeasonDistribution(CreateMatches(pastMatches, teams));
        }

        public Season Sample()
        {
            var pastMatches = this.matches
                .Select(m => new PastMatch(null, m.HomeTeamName, m.AwayTeamName, m.ScoreDistribution.Sample()))
                .ToList();

            return new Season(pastMatches);
        }

        private static IReadOnlyList<IMatch> CreateMatches(IReadOnlyList<PastMatch> pastMatches, IReadOnlyList<Team> teams)
        {
            var averageHomeGoals = Calculator.AverageHomeGoals(pastMatches);
            var averageAwayGoals = Calculator.AverageAwayGoals(pastMatches);

            var matches = new List<IMatch>();

            foreach (var homeTeam in teams)
            {
                foreach (var awayTeam in teams)
                {
                    if (homeTeam != awayTeam)
                    {
                        matches.Add(CreateMatch(pastMatches, homeTeam, awayTeam, averageHomeGoals, averageAwayGoals));
                    }
                }
            }

            return matches;
        }

        private static IMatch CreateMatch(
            IReadOnlyList<PastMatch> pastMatches,
            Team homeTeam,
            Team awayTeam,
            double averageHomeGoals,
            double averageAwayGoals)
        {
            var pastMatch = pastMatches.SingleOrDefault(m => m.HomeTeamName == homeTeam.Name && m.AwayTeamName == awayTeam.Name);

            if (pastMatch != null)
            {
                return pastMatch;
            }

            var expectedScore = Calculator.CalculateExpectedScore(homeTeam, awayTeam, averageHomeGoals, averageAwayGoals);

            return new FutureMatch(homeTeam.Name, awayTeam.Name, expectedScore);
        }
    }
}