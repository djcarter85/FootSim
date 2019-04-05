namespace FootSim.Core
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
        private static readonly IPattern<LocalDate> Pattern = new CompositePatternBuilder<LocalDate>
        {
            {LocalDatePattern.CreateWithInvariantCulture("dd/MM/yyyy"), ld => true},
            {LocalDatePattern.CreateWithInvariantCulture("dd/MM/yy"), ld => true}
        }.Build();

        private readonly string csvFilePath;
        private readonly string url;

        private readonly Lazy<Data> dataLazy;

        public Repository(string season)
        {
            this.csvFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FootSim",
                "epl",
                season,
                "data.csv");
            this.url = $"http://www.football-data.co.uk/mmz4281/{season}/E0.csv";

            this.dataLazy = new Lazy<Data>(this.FetchData);
        }

        public async Task RefreshFromWebAsync()
        {
            var csv = await this.GetCsvDataAsync();

            Directory.CreateDirectory(Path.GetDirectoryName(this.csvFilePath));

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

        public IReadOnlyList<PastMatch> Matches(LocalDate? lastDate)
        {
            if (lastDate == null)
            {
                return this.dataLazy.Value.Matches;
            }
            else
            {
                return this.dataLazy.Value.Matches.Where(m => m.Date <= lastDate).ToArray();
            }
        }

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
            var matches = csvMatches
                 .Select(csvMatch => new PastMatch(
                     Pattern.Parse(csvMatch.Date).GetValueOrThrow(),
                     csvMatch.HomeTeam,
                     csvMatch.AwayTeam,
                     new Score(csvMatch.FTHG, csvMatch.FTAG)))
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