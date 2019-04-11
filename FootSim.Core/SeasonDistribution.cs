namespace FootSim.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Randomness.Distributions;

    public class SeasonDistribution : IDistribution<Season>
    {
        private readonly League league;
        private readonly IReadOnlyList<ISimulatableMatch> matches;

        private SeasonDistribution(League league, IReadOnlyList<ISimulatableMatch> matches)
        {
            this.league = league;
            this.matches = matches;
        }

        public static SeasonDistribution Create(bool resim, Season seasonSoFar)
        {
            return new SeasonDistribution(seasonSoFar.League, CreateMatches(resim, seasonSoFar));
        }

        public Season Sample()
        {
            var pastMatches = this.matches
                .Select(m => new SimulatedMatch(m.HomeTeamName, m.AwayTeamName, m.ScoreDistribution.Sample()))
                .ToList();

            return new Season(this.league, pastMatches);
        }

        private static IReadOnlyList<ISimulatableMatch> CreateMatches(bool resim, Season seasonSoFar)
        {
            var homeConstant = Calculator.CalculateHomeConstant(seasonSoFar);
            var awayConstant = Calculator.CalculateAwayConstant(seasonSoFar);

            var matches = new List<ISimulatableMatch>();

            foreach (var homeTeam in seasonSoFar.Table)
            {
                foreach (var awayTeam in seasonSoFar.Table)
                {
                    if (homeTeam != awayTeam)
                    {
                        matches.Add(CreateMatch(resim, seasonSoFar, homeTeam, awayTeam, homeConstant, awayConstant));
                    }
                }
            }

            return matches;
        }

        private static ISimulatableMatch CreateMatch(
            bool resim,
            Season seasonSoFar,
            TablePlacing homeTeam,
            TablePlacing awayTeam,
            double homeConstant,
            double awayConstant)
        {
            if (!resim)
            {
                var pastMatch = seasonSoFar.Matches.SingleOrDefault(m => m.HomeTeamName == homeTeam.TeamName && m.AwayTeamName == awayTeam.TeamName);

                if (pastMatch != null)
                {
                    return SimulatableMatch.CreateFromCompletedMatch(pastMatch);
                }
            }

            var expectedScore = Calculator.CalculateExpectedScore(homeTeam, awayTeam, homeConstant, awayConstant);

            return SimulatableMatch.CreateFromExpectedScore(homeTeam.TeamName, awayTeam.TeamName, expectedScore);
        }

        private class SimulatableMatch : ISimulatableMatch
        {
            private SimulatableMatch(string homeTeamName, string awayTeamName, IDistribution<Score> scoreDistribution)
            {
                this.HomeTeamName = homeTeamName;
                this.AwayTeamName = awayTeamName;
                this.ScoreDistribution = scoreDistribution;
            }

            public string HomeTeamName { get; }

            public string AwayTeamName { get; }

            public IDistribution<Score> ScoreDistribution { get; }

            public static ISimulatableMatch CreateFromCompletedMatch(ICompletedMatch completedMatch)
            {
                return new SimulatableMatch(
                    completedMatch.HomeTeamName,
                    completedMatch.AwayTeamName,
                    Singleton.Distribution(completedMatch.Score));
            }

            public static ISimulatableMatch CreateFromExpectedScore(string homeTeamName, string awayTeamName, ExpectedScore expectedScore)
            {
                return new SimulatableMatch(homeTeamName, awayTeamName, new ExpectedScoreDistribution(expectedScore));
            }

            private class ExpectedScoreDistribution : IDistribution<Score>
            {
                private readonly ExpectedScore expectedScore;

                public ExpectedScoreDistribution(ExpectedScore expectedScore)
                {
                    this.expectedScore = expectedScore;
                }

                public Score Sample()
                {
                    var homeGoals = Poisson.Distribution(this.expectedScore.Home).Sample();
                    var awayGoals = Poisson.Distribution(this.expectedScore.Away).Sample();

                    return new Score(homeGoals, awayGoals);
                }
            }
        }
    }
}