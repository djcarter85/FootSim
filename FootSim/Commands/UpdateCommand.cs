namespace FootSim.Commands
{
    using System;
    using System.Threading.Tasks;
    using FootSim.Core;
    using FootSim.Options;
    using NodaTime;

    public class UpdateCommand : ICommand
    {
        private readonly UpdateOptions options;
        private readonly IClock clock;

        public UpdateCommand(UpdateOptions options, IClock clock)
        {
            this.options = options;
            this.clock = clock;
        }

        public async Task<ExitCode> ExecuteAsync()
        {
            var league = new League(
                Conversions.ToNation(this.options.Nation),
                this.options.Tier,
                Conversions.ToStartingYear(this.options.StartingYear, this.clock));

            await UpdateFromServer(league);

            return ExitCode.Success;
        }

        public static async Task UpdateFromServer(League league)
        {
            var repository = new Repository(league);

            await repository.UpdateFromServerAsync();

            Console.WriteLine($"League: {league.Description}");
            Console.WriteLine($"Edition: {league.EditionDescription}");
            Console.WriteLine();
            Console.WriteLine("Results have been updated from the server.");
        }
    }
}