namespace FootSim.Core
{
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

        public static double AverageHomeGoals(IReadOnlyList<ICompletedMatch> matches)
        {
            return matches.Average(m => m.Score.Home);
        }

        public static double AverageAwayGoals(IReadOnlyList<ICompletedMatch> matches)
        {
            return matches.Average(m => m.Score.Away);
        }

        public static ExpectedScore CalculateExpectedScore(
            TablePlacing homeTeam,
            TablePlacing awayTeam,
            double averageHomeGoals,
            double averageAwayGoals)
        {
            var expectedHomeGoals = averageHomeGoals * homeTeam.AttackingStrength * awayTeam.DefensiveWeakness;
            var expectedAwayGoals = averageAwayGoals * awayTeam.AttackingStrength * homeTeam.DefensiveWeakness;

            return new ExpectedScore(expectedHomeGoals, expectedAwayGoals);
        }
    }
}