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

        public Repository(League league)
        {
            this.csvFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FootSim",
                GetFolder(league),
                "data.csv");
            this.url = GetUrl(league);

            this.dataLazy = new Lazy<Data>(this.FetchData);
        }

        public async Task UpdateFromServerAsync()
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

        public Season Season(LocalDate? lastDate)
        {
            if (lastDate == null)
            {
                return new Season(this.dataLazy.Value.Matches);
            }
            else
            {
                return new Season(this.dataLazy.Value.Matches.Where(m => m.Date <= lastDate).ToArray());
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

            return new Data(matches);
        }

        private static string GetFolder(League league)
        {
            return $@"{league.Nation}\{league.Tier}\{league.StartingYear}";
        }

        private static string GetUrl(League league)
        {
            return $"http://www.football-data.co.uk/mmz4281/{GetSeasonString(league.StartingYear)}/{GetFileName(league)}.csv";
        }

        private static string GetSeasonString(int startingYear)
        {
            var twoDigitStartingYear = startingYear % 100;

            return $"{twoDigitStartingYear:00}{(twoDigitStartingYear + 1) % 100:00}";
        }

        private static string GetFileName(League league)
        {
            switch (league.Nation)
            {
                case Nation.England:
                    var tierDescription = league.Tier == 4 ? "C" : league.Tier.ToString();
                    return $"E{tierDescription}";
                case Nation.Germany:
                    return $"D{league.Tier + 1}";
                case Nation.Italy:
                    return $"I{league.Tier + 1}";
                case Nation.Spain:
                    return $"SP{league.Tier + 1}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(league), league, null);
            }
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
            public Data(IReadOnlyList<PastMatch> matches)
            {
                this.Matches = matches;
            }

            public IReadOnlyList<PastMatch> Matches { get; }
        }
    }
}