namespace FootballPredictor.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CsvHelper;
    using NodaTime;
    using NodaTime.Text;

    public class Repository
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.CreateWithInvariantCulture("dd/MM/yyyy");

        private readonly string csvFilePath;
        private readonly string url;
        private readonly LocalDate? lastDate;

        private readonly Lazy<Data> dataLazy;

        public Repository(string csvFilePath, string url, LocalDate? lastDate)
        {
            this.csvFilePath = csvFilePath;
            this.url = url;
            this.lastDate = lastDate;

            this.dataLazy = new Lazy<Data>(this.FetchData);
        }

        public async Task RefreshFromWebAsync()
        {
            var csv = await this.GetCsvDataAsync();

            await File.WriteAllTextAsync(this.csvFilePath, csv);
        }

        private async Task<string> GetCsvDataAsync()
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(this.url);
            }
        }

        public IReadOnlyList<string> TeamNames => this.dataLazy.Value.TeamNames;

        public IReadOnlyList<PastMatch> Matches => this.dataLazy.Value.Matches;

        private Data FetchData()
        {
            var csvMatches = this.GetCsvMatches();

            return this.ParseData(csvMatches);
        }

        private IEnumerable<CsvMatch> GetCsvMatches()
        {
            using (var textReader = new StreamReader(this.csvFilePath))
            {
                using (var csvReader = new CsvReader(textReader))
                {
                    return csvReader.GetRecords<CsvMatch>().ToList();
                }
            }
        }

        private Data ParseData(IEnumerable<CsvMatch> csvMatches)
        {
            if (this.lastDate != null)
            {
                csvMatches = csvMatches.Where(cm => Pattern.Parse(cm.Date).GetValueOrThrow() <= this.lastDate);
            }

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
            public string Date { get; set; }

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