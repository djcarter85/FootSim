namespace FootSim.Commands
{
    public static class SeasonExtensions
    {
        public static string ForWeb(this int season)
        {
            var twoDigitSeason = season % 100;

            return $"{twoDigitSeason}{twoDigitSeason + 1}";
        }

        public static string ForDisplay(this int season)
        {
            var twoDigitSeason = season % 100;

            var startYear = twoDigitSeason > 50 ? 1900 + twoDigitSeason : 2000 + twoDigitSeason;

            return $"{startYear}-{startYear + 1}";
        }
    }
}