namespace FootballPredictor.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Season
    {
        public Season(IReadOnlyList<PastMatch> matches)
        {
            this.Matches = matches;
        }

        public IReadOnlyList<PastMatch> Matches { get; }

        public IReadOnlyList<TablePlacing> Table
        {
            get
            {
                var tablePlacings = new Dictionary<string, SettableTablePlacing>();

                foreach (var pastMatch in this.Matches)
                {
                    if (!tablePlacings.ContainsKey(pastMatch.HomeTeamName))
                    {
                        tablePlacings[pastMatch.HomeTeamName] = new SettableTablePlacing();
                    }

                    if (!tablePlacings.ContainsKey(pastMatch.AwayTeamName))
                    {
                        tablePlacings[pastMatch.AwayTeamName] = new SettableTablePlacing();
                    }

                    var homePlacing = tablePlacings[pastMatch.HomeTeamName];
                    var awayPlacing = tablePlacings[pastMatch.AwayTeamName];

                    switch (pastMatch.Score.Result)
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

                    homePlacing.GoalsFor += pastMatch.Score.Home;
                    homePlacing.GoalsAgainst += pastMatch.Score.Away;

                    awayPlacing.GoalsFor += pastMatch.Score.Away;
                    awayPlacing.GoalsAgainst += pastMatch.Score.Home;
                }

                return tablePlacings
                    .OrderByDescending(kvp => kvp.Value.Points)
                    .ThenByDescending(kvp => kvp.Value.GoalDifference)
                    .ThenByDescending(kvp => kvp.Value.GoalsFor)
                    .ThenBy(kvp => kvp.Key)
                    .Select((kvp, pos) => kvp.Value.TablePlacing(pos + 1, kvp.Key))
                    .ToArray();
            }
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

            public TablePlacing TablePlacing(int position, string teamName) =>
                new TablePlacing(position, teamName, this.Won, this.Drawn, this.Lost, this.GoalsFor, this.GoalsAgainst, this.GoalDifference, this.Points);
        }
    }
}