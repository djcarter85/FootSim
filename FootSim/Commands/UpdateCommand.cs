namespace FootSim.Commands
{
    using System.Threading.Tasks;
    using FootSim.Core;
    using FootSim.Options;

    public static class UpdateCommand
    {
        public static async Task<ExitCode> RunAsync(UpdateOptions options)
        {
            var repository = new Repository(options.Season, options.League.ForWeb());

            await repository.RefreshFromWebAsync();

            return ExitCode.Success;
        }
    }
}