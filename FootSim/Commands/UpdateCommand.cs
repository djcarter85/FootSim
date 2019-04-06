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
            var repository = new Repository(this.options.League);

            await repository.UpdateFromServerAsync();

            Console.WriteLine($"League: {this.options.League.Description}");
            Console.WriteLine($"Edition: {this.options.League.EditionDescription}");
            Console.WriteLine();
            Console.WriteLine("Results have been updated from the server.");

            return ExitCode.Success;
        }
    }
}