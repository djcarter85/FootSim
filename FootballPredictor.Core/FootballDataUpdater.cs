namespace FootballPredictor.Core
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class FootballDataUpdater
    {
        private const string Url = "http://www.football-data.co.uk/mmz4281/1819/E0.csv";

        public static async Task UpdateAsync()
        {
            var csv = await GetCsvDataAsync();

            await File.WriteAllTextAsync(Constants.CsvFilePath, csv);
        }

        private static async Task<string> GetCsvDataAsync()
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(Url);
            }
        }
    }
}