namespace FootSim.Core
{
    using System;
    using System.IO;

    public static class Constants
    {
        public static readonly string CsvFilePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FootSim",
                "epl.csv");

        public const string Url = "http://www.football-data.co.uk/mmz4281/1819/E0.csv";
    }
}