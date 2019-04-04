namespace FootballPredictor
{
    using System.Threading.Tasks;
    using CommandLine;
    using FootballPredictor.Sim;

    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<SimOptions>(args)
                .MapResult(
                    async (SimOptions o) => { await SimCommand.RunAsync(o); },
                    errs => Task.FromResult(1));
        }
    }
}
