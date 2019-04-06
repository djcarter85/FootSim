namespace FootSim.Commands
{
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
            var repository = new Repository(this.options.Season.ForWeb(), this.options.League.ForWeb());

            await repository.RefreshFromWebAsync();

            return ExitCode.Success;
        }
    }
}