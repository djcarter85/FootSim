namespace FootballPredictor
{
    using System.Threading.Tasks;
    using CommandLine;
    using FootballPredictor.Sim;
    using FootballPredictor.Update;

    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<SimOptions, UpdateOptions>(args)
                .MapResult(
                    async (SimOptions o) => { await SimCommand.RunAsync(o); },
                    async (UpdateOptions o) => { await UpdateCommand.RunAsync(o); },
                    errs => Task.FromResult(1));
        }
    }
}
