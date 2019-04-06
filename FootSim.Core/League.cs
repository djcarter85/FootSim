namespace FootSim.Core
{
    using System;

    public class League
    {
        public League(Nation nation, int tier, int startingYear)
        {
            this.Nation = nation;
            this.Tier = tier;
            this.StartingYear = startingYear;
        }

        public Nation Nation { get; }

        public int Tier { get; }

        public int StartingYear { get; }

        public string Description => $"{this.TierDescription} ({this.Nation})";

        public string EditionDescription
        {
            get
            {
                var twoDigitSeason = this.StartingYear % 100;

                var startYear = twoDigitSeason > 50 ? 1900 + twoDigitSeason : 2000 + twoDigitSeason;

                return $"{startYear}-{startYear + 1}";
            }
        }

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
                }

                throw new ArgumentOutOfRangeException();
            }
        }
    }
}