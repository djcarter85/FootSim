namespace FootSim.Commands
{
    using System;
    using FootSim.Core;
    using FootSim.Options;
    using NodaTime;

    public static class Conversions
    {
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

        public static int ToStartingYear(int? startingYearOption)
        {
            if (startingYearOption == null)
            {
                var currentDate = SystemClock.Instance.GetCurrentInstant().InZone(UkTimeZone).LocalDateTime.Date;
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
    }
}