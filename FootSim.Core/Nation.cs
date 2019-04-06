namespace FootSim.Core
{
    public class Nation
    {
        private Nation(string displayName, string representation)
        {
            this.DisplayName = displayName;
            this.Representation = representation;
        }

        public string DisplayName { get; }

        public string Representation { get; }

        public static Nation England { get; } = new Nation("England", "England");

        public static Nation Germany { get; } = new Nation("Germany", "Germany");

        public static Nation Italy { get; } = new Nation("Italy", "Italy");

        public static Nation Spain { get; } = new Nation("Spain", "Spain");

        public static Nation France { get; } = new Nation("France", "France");
    }
}