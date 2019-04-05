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
                default:
                    throw new ArgumentOutOfRangeException(nameof(league), league, null);
            }
        }
    }
}