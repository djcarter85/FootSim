namespace FootballPredictor
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Calculator
    {
        public static int GoalsScored(IReadOnlyList<PastMatch> matches, string teamName)
        {
            var goalsScoredAtHome = matches
                .Where(m => m.HomeTeamName == teamName)
                .Sum(m => m.Score.Home);

            var goalsScoredAway = matches
                .Where(m => m.AwayTeamName == teamName)
                .Sum(m => m.Score.Away);

            return goalsScoredAtHome + goalsScoredAway;
        }

        public static int GoalsConceded(IReadOnlyList<PastMatch> matches, string teamName)
        {
            var goalsConcededAtHome = matches
                .Where(m => m.HomeTeamName == teamName)
                .Sum(m => m.Score.Away);

            var goalsConcededAway = matches
                .Where(m => m.AwayTeamName == teamName)
                .Sum(m => m.Score.Home);

            return goalsConcededAtHome + goalsConcededAway;
        }

        public static int CalculateTotalGoalsScored(IReadOnlyList<PastMatch> matches)
        {
            var goalsScoredAtHome = matches.Sum(m => m.Score.Home);

            var goalsScoredAway = matches.Sum(m => m.Score.Away);

            return goalsScoredAtHome + goalsScoredAway;
        }

        public static double AverageHomeGoals(IReadOnlyList<PastMatch> matches)
        {
            return matches.Average(m => m.Score.Home);
        }

        public static double AverageAwayGoals(IReadOnlyList<PastMatch> matches)
        {
            return matches.Average(m => m.Score.Away);
        }

        public static IReadOnlyList<Team> GetTeams(IReadOnlyList<string> teamNames, IReadOnlyList<PastMatch> matches)
        {
            var totalGoalsScored = CalculateTotalGoalsScored(matches);
            var averageGoalsScored = totalGoalsScored / teamNames.Count;

            return teamNames.Select(t => Team.Create(t, matches, averageGoalsScored)).ToArray();
        }

        public static ExpectedScore CalculateExpectedScore(
            Team homeTeam,
            Team awayTeam,
            double averageHomeGoals,
            double averageAwayGoals)
        {
            var expectedHomeGoals = averageHomeGoals * homeTeam.AttackingStrength * awayTeam.DefensiveWeakness;
            var expectedAwayGoals = averageAwayGoals * awayTeam.AttackingStrength * homeTeam.DefensiveWeakness;

            return new ExpectedScore(expectedHomeGoals, expectedAwayGoals);
        }
    }
}