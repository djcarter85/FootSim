namespace FootSim.Core
{
    using System.Collections.Generic;

    public class Team
    {
        private Team(string name, int goalsScored, int goalsConceded, double attackingStrength, double defensiveWeakness)
        {
            this.Name = name;
            this.GoalsScored = goalsScored;
            this.GoalsConceded = goalsConceded;
            this.AttackingStrength = attackingStrength;
            this.DefensiveWeakness = defensiveWeakness;
        }

        public string Name { get; }

        public int GoalsScored { get; }

        public int GoalsConceded { get; }

        public double AttackingStrength { get; }

        public double DefensiveWeakness { get; }

        public static Team Create(string teamName, IReadOnlyList<PastMatch> matches, double averageGoalsScored)
        {
            var goalsScored = Calculator.GoalsScored(matches, teamName);
            var goalsConceded = Calculator.GoalsConceded(matches, teamName);

            var attackingStrength = goalsScored / averageGoalsScored;
            var defensiveWeakness = goalsConceded / averageGoalsScored;

            return new Team(teamName, goalsScored, goalsConceded, attackingStrength, defensiveWeakness);
        }
    }
}