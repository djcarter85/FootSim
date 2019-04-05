namespace FootSim
{
    using System.Threading.Tasks;
    using CommandLine;
    using FootSim.Sim;
    using FootSim.Update;

    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var exitCode = await Parser.Default.ParseArguments<SimOptions, UpdateOptions>(args)
                .MapResult(
                    async (SimOptions o) => await SimCommand.RunAsync(o),
                    async (UpdateOptions o) => await UpdateCommand.RunAsync(o),
                    errs => Task.FromResult(ExitCode.Failure));

            return (int)exitCode;
        }
    }
}
