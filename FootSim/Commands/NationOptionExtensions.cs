namespace FootSim.Commands
{
    using System;
    using FootSim.Core;
    using FootSim.Options;

    public static class NationOptionExtensions
    {
        public static Nation ToNation(this NationOption nationOption)
        {
            switch (nationOption)
            {
                case NationOption.Eng:
                    return Nation.England;
                case NationOption.Ger:
                    return Nation.Germany;
                case NationOption.Ita:
                    return Nation.Italy;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nationOption), nationOption, null);
            }
        }
    }
}