namespace FootballPredictor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;

    public class Repository
    {
        private readonly Data data;

        public Repository()
        {
            var csvMatches = GetCsvMatches();

            this.data = ParseData(csvMatches);
        }

        public IReadOnlyList<string> TeamNames => this.data.TeamNames;

        public IReadOnlyList<Match> Matches => this.data.Matches;

        private static IEnumerable<CsvMatch> GetCsvMatches()
        {
            using (var textReader = new StreamReader(Constants.CsvFilePath))
            {
                using (var csvReader = new CsvReader(textReader))
                {
                    return csvReader.GetRecords<CsvMatch>().ToList();
                }
            }
        }

        private static Data ParseData(IEnumerable<CsvMatch> csvMatches)
        {
            var matches = csvMatches
                .Select(csvMatch => new Match(csvMatch.HomeTeam, csvMatch.AwayTeam, csvMatch.FTHG, csvMatch.FTAG))
                .ToList();

            var teamNames = matches
                .SelectMany(m => new[] { m.HomeTeamName, m.AwayTeamName })
                .Distinct()
                .ToList();

            return new Data(teamNames, matches);
        }

        private class CsvMatch
        {
            public string HomeTeam { get; set; }

            public string AwayTeam { get; set; }

            public int FTHG { get; set; }

            public int FTAG { get; set; }
        }

        private class Data
        {
            public Data(IReadOnlyList<string> teamNames, IReadOnlyList<Match> matches)
            {
                this.TeamNames = teamNames;
                this.Matches = matches;
            }

            public IReadOnlyList<string> TeamNames { get; }

            public IReadOnlyList<Match> Matches { get; }
        }
    }
}