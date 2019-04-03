namespace FootballPredictor
{
    using System;
    using System.Text;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var seasonDistribution = SeasonSimulator.CreateSeasonDistribution();

            do
            {
                Console.WriteLine(GetDescription(seasonDistribution.Sample()));
            } while (string.IsNullOrEmpty(Console.ReadLine()));
        }

        private static string GetDescription(Season season)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{"#",2} {"Team",-15} {"GD",-3}  {"PTS",-3}");

            int index = 1;
            foreach (var tablePlacing in season.Table)
            {
                stringBuilder.AppendLine($"{index,2} {tablePlacing.TeamName,-15} {tablePlacing.GoalDifference,3}  {tablePlacing.Points,3}");

                if (index == 4 || index == 17)
                {
                    stringBuilder.AppendLine(new string('-', 27));
                }

                index++;
            }

            return stringBuilder.ToString();
        }
    }
}
