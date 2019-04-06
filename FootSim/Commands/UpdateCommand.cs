namespace FootSim.Commands
{
    using System;
    using System.Threading.Tasks;
    using FootSim.Core;
    using FootSim.Options;

    public class UpdateCommand : ICommand
    {
        private readonly UpdateOptions options;

        public UpdateCommand(UpdateOptions options)
        {
            this.options = options;
        }

        public async Task<ExitCode> ExecuteAsync()
        {
            await UpdateFromServer(this.options.League);

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