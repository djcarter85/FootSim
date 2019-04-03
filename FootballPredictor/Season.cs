namespace FootballPredictor
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
                    .Select(kvp => kvp.Value.TablePlacing(kvp.Key))
                    .OrderByDescending(tp => tp.Points)
                    .ThenByDescending(tp => tp.GoalDifference)
                    .ThenByDescending(tp => tp.GoalsFor)
                    .ThenBy(tp => tp.TeamName)
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

            public TablePlacing TablePlacing(string teamName) =>
                new TablePlacing(teamName, this.Won, this.Drawn, this.Lost, this.GoalsFor, this.GoalsAgainst);
        }
    }
}