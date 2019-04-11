namespace FootSim.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Calculator
    {
        public static int GoalsScored(IReadOnlyList<ICompletedMatch> matches, string teamName)
        {
            var goalsScoredAtHome = matches
                .Where(m => m.HomeTeamName == teamName)
                .Sum(m => m.Score.Home);

            var goalsScoredAway = matches
                .Where(m => m.AwayTeamName == teamName)
                .Sum(m => m.Score.Away);

            return goalsScoredAtHome + goalsScoredAway;
        }

        public static int GoalsConceded(IReadOnlyList<ICompletedMatch> matches, string teamName)
        {
            var goalsConcededAtHome = matches
                .Where(m => m.HomeTeamName == teamName)
                .Sum(m => m.Score.Away);

            var goalsConcededAway = matches
                .Where(m => m.AwayTeamName == teamName)
                .Sum(m => m.Score.Home);

            return goalsConcededAtHome + goalsConcededAway;
        }

        public static int CalculateTotalGoalsScored(IReadOnlyList<ICompletedMatch> matches)
        {
            var goalsScoredAtHome = matches.Sum(m => m.Score.Home);

            var goalsScoredAway = matches.Sum(m => m.Score.Away);

            return goalsScoredAtHome + goalsScoredAway;
        }

        public static double CalculateHomeConstant(Season seasonSoFar)
        {
            return CalculateConstant(seasonSoFar, m => m.Score.Home);
        }

        public static double CalculateAwayConstant(Season seasonSoFar)
        {
            return CalculateConstant(seasonSoFar, m => m.Score.Away);
        }

        private static double CalculateConstant(Season seasonSoFar, Func<ICompletedMatch, int> getGoalsScoredPerMatch)
        {
            var numberOfTeams = seasonSoFar.Table.Count;

            var goalsScoredSoFar = seasonSoFar.Matches.Sum(getGoalsScoredPerMatch);

            var endOfSeasonTotalMatches = numberOfTeams * (numberOfTeams - 1);

            var matchesPlayedSoFar = seasonSoFar.Matches.Count;

            var expectedEndOfSeasonGoalsScored =
                (double)goalsScoredSoFar * endOfSeasonTotalMatches / matchesPlayedSoFar;

            var x = seasonSoFar.Table.Sum(tp => tp.AttackingStrength * tp.DefensiveWeakness);

            return expectedEndOfSeasonGoalsScored / (numberOfTeams * numberOfTeams - x);
        }

        public static ExpectedScore CalculateExpectedScore(
            TablePlacing homeTeam,
            TablePlacing awayTeam,
            double homeConstant,
            double awayConstant)
        {
            var expectedHomeGoals = homeConstant * homeTeam.AttackingStrength * awayTeam.DefensiveWeakness;
            var expectedAwayGoals = awayConstant * awayTeam.AttackingStrength * homeTeam.DefensiveWeakness;

            return new ExpectedScore(expectedHomeGoals, expectedAwayGoals);
        }
    }
}