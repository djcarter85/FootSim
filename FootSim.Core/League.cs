namespace FootSim.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;

    public class League
    {
        private readonly Lazy<IReadOnlyList<PositionGrouping>> positionGroupingsLazy;
        private readonly Lazy<string> leagueNameLazy;
        private readonly Lazy<string> fileNameLazy;

        public League(Nation nation, int tier, int startingYear)
        {
            this.Nation = nation;
            this.Tier = tier;
            this.StartingYear = startingYear;

            this.positionGroupingsLazy = new Lazy<IReadOnlyList<PositionGrouping>>(this.FetchPositionGroupings);
            this.leagueNameLazy = new Lazy<string>(this.FetchLeagueName);
            this.fileNameLazy = new Lazy<string>(this.FetchFileName);
        }

        public Nation Nation { get; }

        public int Tier { get; }

        public int StartingYear { get; }

        public string Description => $"{this.LeagueName} ({this.Nation})";

        public string EditionDescription => $"{this.StartingYear}-{this.StartingYear + 1}";

        public IEnumerable<PositionGrouping> PositionGroupings => this.positionGroupingsLazy.Value;

        public string FileName => this.fileNameLazy.Value;

        private string LeagueName => this.leagueNameLazy.Value;

        private IReadOnlyList<PositionGrouping> FetchPositionGroupings()
        {
            using (Stream stream = CsvEmbeddedResources.GetStream("PositionGroupings.csv"))
            {
                using (var textReader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(textReader))
                    {
                        return csvReader.GetRecords<CsvPositionGrouping>()
                            .Where(cpg =>
                                cpg.Nation == this.Nation.ToString() &&
                                cpg.Tier == this.Tier &&
                                cpg.StartingYear == this.StartingYear)
                            .Select(cpg => new PositionGrouping(cpg.LongName, cpg.ShortName, cpg.Min, cpg.Max))
                            .ToArray();
                    }
                }
            }
        }

        private string FetchLeagueName()
        {
            using (Stream stream = CsvEmbeddedResources.GetStream("LeagueNames.csv"))
            {
                using (var textReader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(textReader))
                    {
                        var csvLeagueName = csvReader.GetRecords<CsvLeagueName>()
                            .SingleOrDefault(cln =>
                                cln.Nation == this.Nation.ToString() &&
                                cln.Tier == this.Tier &&
                                cln.Contains(this.StartingYear));

                        return csvLeagueName?.Name ?? $"Tier {this.Tier}";
                    }
                }
            }
        }

        private string FetchFileName()
        {
            using (Stream stream = CsvEmbeddedResources.GetStream("FileNames.csv"))
            {
                using (var textReader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(textReader))
                    {
                        var csvFileName = csvReader.GetRecords<CsvFileName>()
                            .Single(cfn =>
                                cfn.Nation == this.Nation.ToString() &&
                                cfn.Tier == this.Tier);

                        return csvFileName.FileName;
                    }
                }
            }
        }

        private class CsvPositionGrouping
        {
            public string Nation { get; set; }

            public int Tier { get; set; }

            public int StartingYear { get; set; }

            public string LongName { get; set; }

            public string ShortName { get; set; }

            public int Min { get; set; }

            public int Max { get; set; }
        }

        private class CsvLeagueName
        {
            public string Nation { get; set; }

            public int Tier { get; set; }

            public int? StartYear { get; set; }

            public int? EndYear { get; set; }

            public string Name { get; set; }

            public bool Contains(int year) =>
                (this.StartYear == null || this.StartYear <= year) && (this.EndYear == null || year < this.EndYear);
        }

        private class CsvFileName
        {
            public string Nation { get; set; }

            public int Tier { get; set; }

            public string FileName { get; set; }
        }
    }
}