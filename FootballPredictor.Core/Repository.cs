namespace FootballPredictor.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;

    public class Repository
    {
        private readonly Lazy<Data> dataLazy;

        public Repository(string csvFilePath)
        {
            this.dataLazy = new Lazy<Data>(() => FetchData(csvFilePath));
        }

        public IReadOnlyList<string> TeamNames => this.dataLazy.Value.TeamNames;

        public IReadOnlyList<PastMatch> Matches => this.dataLazy.Value.Matches;

        private static Data FetchData(string csvFilePath)
        {
            var csvMatches = GetCsvMatches(csvFilePath);

            return ParseData(csvMatches);
        }

        private static IEnumerable<CsvMatch> GetCsvMatches(string csvFilePath)
        {
            using (var textReader = new StreamReader(csvFilePath))
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
                .Select(csvMatch => new PastMatch(csvMatch.HomeTeam, csvMatch.AwayTeam, new Score(csvMatch.FTHG, csvMatch.FTAG)))
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
            public Data(IReadOnlyList<string> teamNames, IReadOnlyList<PastMatch> matches)
            {
                this.TeamNames = teamNames;
                this.Matches = matches;
            }

            public IReadOnlyList<string> TeamNames { get; }

            public IReadOnlyList<PastMatch> Matches { get; }
        }
    }
}