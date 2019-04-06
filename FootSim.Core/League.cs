namespace FootSim.Core
{
    using System;
    using System.Collections.Generic;

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

        public IEnumerable<PositionGrouping> PositionGroupings
        {
            get
            {
                switch (this.Nation)
                {
                    case Nation.England:
                        switch (this.Tier)
                        {
                            case 0:
                                yield return new PositionGrouping("Champions League", "UCL", 1, 2, 3, 4);
                                yield return new PositionGrouping("Europa League", "UEL", 5, 6, 7);
                                yield return new PositionGrouping("Relegation", "Rel", 18, 19, 20);
                                break;
                            case 1:
                                yield return new PositionGrouping("Automatic promotion", "Top 2", 1, 2);
                                yield return new PositionGrouping("Playoffs", "PO", 3, 4, 5, 6);
                                yield return new PositionGrouping("Relegation", "Rel", 22, 23, 24);
                                break;
                            case 2:
                                yield return new PositionGrouping("Automatic promotion", "Top 2", 1, 2);
                                yield return new PositionGrouping("Playoffs", "PO", 3, 4, 5, 6);
                                yield return new PositionGrouping("Relegation", "Rel", 22, 23, 24);
                                break;
                            case 3:
                                yield return new PositionGrouping("Automatic promotion", "Top 3", 1, 2, 3);
                                yield return new PositionGrouping("Playoffs", "PO", 4, 5, 6, 7);
                                yield return new PositionGrouping("Relegation", "Rel", 23, 24);
                                break;
                        }

                        break;
                    case Nation.Germany:
                        switch (this.Tier)
                        {
                            case 0:
                                yield return new PositionGrouping("Champions League", "UCL", 1, 2, 3, 4);
                                yield return new PositionGrouping("Europa League", "UEL", 5, 6);
                                yield return new PositionGrouping("Relegation playoff", "Rel PO", 16);
                                yield return new PositionGrouping("Relegation", "Rel", 17, 18);
                                break;
                            case 1:
                                yield return new PositionGrouping("Automatic promotion", "Top 2", 1, 2);
                                yield return new PositionGrouping("Playoff", "PO", 3);
                                yield return new PositionGrouping("Relegation playoff", "Rel PO", 16);
                                yield return new PositionGrouping("Relegation", "Rel", 17, 18);
                                break;
                        }

                        break;
                    case Nation.Italy:
                        break;
                    case Nation.Spain:
                        break;
                    case Nation.France:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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