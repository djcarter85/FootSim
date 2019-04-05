namespace FootSim.Commands
{
    using System;
    using FootSim.Options;

    public static class LeagueExtensions
    {
        public static string GetCode(this League league)
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
    }
}