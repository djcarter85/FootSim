namespace FootballPredictor
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Calculator
    {
        public static double CalculateAverageHomeScore(IReadOnlyList<Match> matches)
        {
            return matches.Select(m => m.HomeScore).Average();
        }

        public static double CalculateAverageAwayScore(IReadOnlyList<Match> matches)
        {
            return matches.Select(m => m.AwayScore).Average();
        }

        public static int GoalsScored(IReadOnlyList<Match> matches, string teamName)
        {
            var goalsScoredAtHome = matches
                .Where(m => m.HomeTeamName == teamName)
                .Sum(m => m.HomeScore);

            var goalsScoredAway = matches
                .Where(m => m.AwayTeamName == teamName)
                .Sum(m => m.AwayScore);

            return goalsScoredAtHome + goalsScoredAway;
        }

        public static int GoalsConceded(IReadOnlyList<Match> matches, string teamName)
        {
            var goalsConcededAtHome = matches
                .Where(m => m.HomeTeamName == teamName)
                .Sum(m => m.AwayScore);

            var goalsConcededAway = matches
                .Where(m => m.AwayTeamName == teamName)
                .Sum(m => m.HomeScore);

            return goalsConcededAtHome + goalsConcededAway;
        }

        public static int CalculateTotalGoalsScored(IReadOnlyList<Match> matches)
        {
            var goalsScoredAtHome = matches.Sum(m => m.HomeScore);

            var goalsScoredAway = matches.Sum(m => m.AwayScore);

            return goalsScoredAtHome + goalsScoredAway;
        }

        public static int HomeGoalsScored(IReadOnlyList<Match> matches)
        {
            return matches.Sum(m => m.HomeScore);
        }

        public static int AwayGoalsScored(IReadOnlyList<Match> matches)
        {
            return matches.Sum(m => m.AwayScore);
        }

        public static double AverageHomeGoals(IReadOnlyList<Match> matches)
        {
            return matches.Average(m => m.HomeScore);
        }

        public static double AverageAwayGoals(IReadOnlyList<Match> matches)
        {
            return matches.Average(m => m.AwayScore);
        }

        public static IReadOnlyDictionary<string, Team> GetTeams(IReadOnlyList<string> teamNames, IReadOnlyList<Match> matches)
        {
            var totalGoalsScored = CalculateTotalGoalsScored(matches);
            var averageGoalsScored = totalGoalsScored / teamNames.Count;

            return teamNames.ToDictionary(t => t, t => Team.Create(t, matches, averageGoalsScored));
        }

        public static ExpectedScore CalculateExpectedScore(
            string homeTeamName,
            string awayTeamName,
            IReadOnlyDictionary<string, Team> teams,
            double averageHomeGoals,
            double averageAwayGoals)
        {
            var homeTeam = teams[homeTeamName];
            var awayTeam = teams[awayTeamName];

            var expectedHomeGoals = averageHomeGoals * homeTeam.AttackingStrength * awayTeam.DefensiveWeakness;
            var expectedAwayGoals = averageAwayGoals * awayTeam.AttackingStrength * homeTeam.DefensiveWeakness;

            return new ExpectedScore(expectedHomeGoals, expectedAwayGoals);
        }
    }
}