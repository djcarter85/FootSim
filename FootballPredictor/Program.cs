namespace FootballPredictor
{
    using System;
    using System.Threading.Tasks;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await FootballDataUpdater.UpdateAsync();

            Console.ReadLine();
        }
    }
}
