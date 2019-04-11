namespace FootSim.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Season
    {
        private readonly Lazy<IReadOnlyList<TablePlacing>> tableLazy;

        public Season(League league, IReadOnlyList<ICompletedMatch> matches)
        {
            this.League = league;
            this.Matches = matches;

            this.tableLazy = new Lazy<IReadOnlyList<TablePlacing>>(this.CalculateTablePlacings);
        }

        public League League { get; }

        public IReadOnlyList<ICompletedMatch> Matches { get; }

        public IReadOnlyList<TablePlacing> Table => this.tableLazy.Value;

        private IReadOnlyList<TablePlacing> CalculateTablePlacings()
        {
            var tablePlacings = new Dictionary<string, SettableTablePlacing>();

            foreach (var simulatedMatch in this.Matches)
            {
                if (!tablePlacings.ContainsKey(simulatedMatch.HomeTeamName))
                {
                    tablePlacings[simulatedMatch.HomeTeamName] = new SettableTablePlacing();
                }

                if (!tablePlacings.ContainsKey(simulatedMatch.AwayTeamName))
                {
                    tablePlacings[simulatedMatch.AwayTeamName] = new SettableTablePlacing();
                }

                var homePlacing = tablePlacings[simulatedMatch.HomeTeamName];
                var awayPlacing = tablePlacings[simulatedMatch.AwayTeamName];

                switch (simulatedMatch.Score.Result)
                {
                    case Result.HomeWin:
                        homePlacing.Won++;
                        awayPlacing.Lost++;
                        break;
                    case Result.Draw:
                        homePlacing.Drawn++;
                        awayPlacing.Drawn++;
                        break;
                    case Result.AwayWin:
                        homePlacing.Lost++;
                        awayPlacing.Won++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                homePlacing.GoalsFor += simulatedMatch.Score.Home;
                homePlacing.GoalsAgainst += simulatedMatch.Score.Away;

                awayPlacing.GoalsFor += simulatedMatch.Score.Away;
                awayPlacing.GoalsAgainst += simulatedMatch.Score.Home;
            }

            var averageGoalsScored = tablePlacings.Average(tp => tp.Value.GoalsFor);

            return tablePlacings
                .OrderByDescending(kvp => kvp.Value.Points)
                .ThenByDescending(kvp => kvp.Value.GoalDifference)
                .ThenByDescending(kvp => kvp.Value.GoalsFor)
                .ThenBy(kvp => kvp.Key)
                .Select((kvp, pos) => kvp.Value.TablePlacing(pos + 1, kvp.Key, averageGoalsScored))
                .ToArray();
        }

        private class SettableTablePlacing
        {
            public int Won { get; set; }

            public int Drawn { get; set; }

            public int Lost { get; set; }

            public int GoalsFor { get; set; }

            public int GoalsAgainst { get; set; }

            public int GoalDifference => this.GoalsFor - this.GoalsAgainst;

            public int Points => 3 * this.Won + this.Drawn;

            public TablePlacing TablePlacing(int position, string teamName, double averageGoalsScored) =>
                new TablePlacing(
                    position,
                    teamName,
                    this.Won + this.Drawn + this.Lost,
                    this.Won,
                    this.Drawn,
                    this.Lost,
                    this.GoalsFor,
                    this.GoalsAgainst,
                    this.GoalDifference,
                    this.Points,
                    this.GoalsFor / averageGoalsScored,
                    this.GoalsAgainst / averageGoalsScored);
        }
    }
}