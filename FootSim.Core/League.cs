namespace FootSim.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CsvHelper;

    public class League
    {
        private readonly Lazy<IReadOnlyList<PositionGrouping>> positionGroupingsLazy;

        private IReadOnlyList<PositionGrouping> FetchPositionGroupings()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FootSim.Core.PositionGroupings.csv"))
            {
                using (var textReader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(textReader))
                    {
                        return csvReader.GetRecords<CsvPositionGrouping>()
                            .Where(cpg =>
                                cpg.Nation == this.Nation.ToString() && cpg.Tier == this.Tier &&
                                cpg.StartingYear == this.StartingYear)
                            .Select(cpg => new PositionGrouping(cpg.LongName, cpg.ShortName, cpg.Min, cpg.Max))
                            .ToArray();
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

        public League(Nation nation, int tier, int startingYear)
        {
            this.Nation = nation;
            this.Tier = tier;
            this.StartingYear = startingYear;

            this.positionGroupingsLazy = new Lazy<IReadOnlyList<PositionGrouping>>(this.FetchPositionGroupings);
        }

        public Nation Nation { get; }

        public int Tier { get; }

        public int StartingYear { get; }

        public string Description => $"{this.TierDescription} ({this.Nation})";

        public string EditionDescription => $"{this.StartingYear}-{this.StartingYear + 1}";

        public IEnumerable<PositionGrouping> PositionGroupings => this.positionGroupingsLazy.Value;

        private string TierDescription
        {
            get
            {
                switch (this.Nation)
                {
                    case Nation.England:
                        switch (this.Tier)
                        {
                            case 0:
                                return "Premier League";
                            case 1:
                                return "Championship";
                            case 2:
                                return "League One";
                            case 3:
                                return "League Two";
                            case 4:
                                return "National League";
                        }

                        break;
                    case Nation.Germany:
                        switch (this.Tier)
                        {
                            case 0:
                                return "Bundesliga 1";
                            case 1:
                                return "Bundesliga 2";
                        }

                        break;
                    case Nation.Italy:
                        switch (this.Tier)
                        {
                            case 0:
                                return "Serie A";
                            case 1:
                                return "Serie B";
                        }

                        break;
                    case Nation.Spain:
                        switch (this.Tier)
                        {
                            case 0:
                                return "La Liga Primera";
                            case 1:
                                return "La Liga Segunda";
                        }

                        break;
                    case Nation.France:
                        switch (this.Tier)
                        {
                            case 0:
                                return "Ligue 1";
                            case 1:
                                return "Ligue 2";
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                throw new ArgumentOutOfRangeException();
            }
        }
    }
}