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

        public IReadOnlyList<string> Teams => this.data.Teams;

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

            var teams = matches
                .SelectMany(m => new[] { m.HomeTeam, m.AwayTeam })
                .Distinct()
                .ToList();

            return new Data(teams, matches);
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
            public Data(IReadOnlyList<string> teams, IReadOnlyList<Match> matches)
            {
                this.Teams = teams;
                this.Matches = matches;
            }

            public IReadOnlyList<string> Teams { get; }

            public IReadOnlyList<Match> Matches { get; }
        }
    }
}