namespace FootSim.Commands
{
    using System;
    using FootSim.Options;

    public static class LeagueExtensions
    {
        public static string ForWeb(this League league)
        {
            switch (league)
            {
                case League.Epl:
                    return "E0";
                case League.Champ:
                    return "E1";
                case League.L1:
                    return "E2";
                case League.L2:
                    return "E3";
                case League.Conf:
                    return "EC";
                default:
                    throw new ArgumentOutOfRangeException(nameof(league), league, null);
            }
        }

        public static string ForDisplay(this League league)
        {
            switch (league)
            {
                case League.Epl:
                    return "English Premier League";
                case League.Champ:
                    return "English Championship";
                case League.L1:
                    return "English League One";
                case League.L2:
                    return "English League Two";
                case League.Conf:
                    return "English Conference";
                default:
                    throw new ArgumentOutOfRangeException(nameof(league), league, null);
            }
        }
    }
}