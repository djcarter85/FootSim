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
    }
}