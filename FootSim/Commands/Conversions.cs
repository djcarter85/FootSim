namespace FootSim.Commands
{
    using System;
    using FootSim.Core;
    using FootSim.Options;

    public static class Conversions
    {
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

        public static int ToStartingYear(int startingYearOption)
        {
            var twoDigitStartingYear = startingYearOption % 100;

            return twoDigitStartingYear > 50 ? 1900 + twoDigitStartingYear : 2000 + twoDigitStartingYear;
        }
    }
}