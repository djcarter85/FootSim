namespace FootSim.Commands
{
    using System;
    using FootSim.Core;
    using FootSim.Options;
    using NodaTime;
    using NodaTime.Text;

    public static class Conversions
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.Iso;
        private static readonly DateTimeZone UkTimeZone = DateTimeZoneProviders.Tzdb["Europe/London"];

        public static Nation ToNation(NationOption nationOption)
        {
            switch (nationOption)
            {
                case NationOption.Eng:
                    return Nation.England;
                case NationOption.Ger:
                    return Nation.Germany;
                case NationOption.Ita:
                    return Nation.Italy;
                case NationOption.Spa:
                    return Nation.Spain;
                case NationOption.Fra:
                    return Nation.France;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nationOption), nationOption, null);
            }
        }

        public static int ToStartingYear(int? startingYearOption, IClock clock)
        {
            if (startingYearOption == null)
            {
                var currentDate = clock.GetCurrentInstant().InZone(UkTimeZone).LocalDateTime.Date;
                const int august = 8;
                if (currentDate.Month < august)
                {
                    return currentDate.Year - 1;
                }
                else
                {
                    return currentDate.Year;
                }
            }

            if (startingYearOption.Value < 0)
            {
                throw new ArgumentException(nameof(startingYearOption.Value));
            }

            if (startingYearOption.Value < 90)
            {
                return 2000 + startingYearOption.Value;
            }

            if (startingYearOption.Value < 100)
            {
                return 1900 + startingYearOption.Value;
            }

            return startingYearOption.Value;
        }

        public static LocalDate? ToDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }

            return Pattern.Parse(dateString).GetValueOrThrow();
        }
    }
}