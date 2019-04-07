namespace FootSim.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Randomness.Distributions;
    using Randomness.Distributions.Discrete;

    public class SeasonDistribution : IDistribution<Season>
    {
        private readonly League league;
        private readonly IReadOnlyList<ISimulatableMatch> matches;

        private SeasonDistribution(League league, IReadOnlyList<ISimulatableMatch> matches)
        {
            this.league = league;
            this.matches = matches;
        }

        public static SeasonDistribution Create(Season seasonSoFar)
        {
            return new SeasonDistribution(seasonSoFar.League, CreateMatches(seasonSoFar));
        }

        public Season Sample()
        {
            var pastMatches = this.matches
                .Select(m => new SimulatedMatch(m.HomeTeamName, m.AwayTeamName, m.ScoreDistribution.Sample()))
                .ToList();

            return new Season(this.league, pastMatches);
        }

        private static IReadOnlyList<ISimulatableMatch> CreateMatches(Season seasonSoFar)
        {
            var averageHomeGoals = Calculator.AverageHomeGoals(seasonSoFar.Matches);
            var averageAwayGoals = Calculator.AverageAwayGoals(seasonSoFar.Matches);

            var matches = new List<ISimulatableMatch>();

            foreach (var homeTeam in seasonSoFar.Table)
            {
                foreach (var awayTeam in seasonSoFar.Table)
                {
                    if (homeTeam != awayTeam)
                    {
                        matches.Add(CreateMatch(seasonSoFar, homeTeam, awayTeam, averageHomeGoals, averageAwayGoals));
                    }
                }
            }

            return matches;
        }

        private static ISimulatableMatch CreateMatch(
            Season seasonSoFar,
            TablePlacing homeTeam,
            TablePlacing awayTeam,
            double averageHomeGoals,
            double averageAwayGoals)
        {
            var pastMatch = seasonSoFar.Matches.SingleOrDefault(m => m.HomeTeamName == homeTeam.TeamName && m.AwayTeamName == awayTeam.TeamName);

            if (pastMatch != null)
            {
                return new SimulatableMatch(pastMatch.HomeTeamName, pastMatch.AwayTeamName, Singleton<Score>.Distribution(pastMatch.Score));
            }

            var expectedScore = Calculator.CalculateExpectedScore(homeTeam, awayTeam, averageHomeGoals, averageAwayGoals);

            var scoreDistribution =
                from homeGoals in Poisson.Distribution(expectedScore.Home)
                from awayGoals in Poisson.Distribution(expectedScore.Away)
                select new Score(homeGoals, awayGoals);

            return new SimulatableMatch(homeTeam.TeamName, awayTeam.TeamName, scoreDistribution);
        }

        private class SimulatableMatch : ISimulatableMatch
        {
            public SimulatableMatch(string homeTeamName, string awayTeamName, IDistribution<Score> scoreDistribution)
            {
                this.HomeTeamName = homeTeamName;
                this.AwayTeamName = awayTeamName;
                this.ScoreDistribution = scoreDistribution;
            }

            public string HomeTeamName { get; }

            public string AwayTeamName { get; }

            public IDistribution<Score> ScoreDistribution { get; }
        }
    }
}