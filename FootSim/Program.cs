namespace FootSim
{
    using System;
    using System.Threading.Tasks;
    using CommandLine;
    using FootSim.Commands;
    using FootSim.Options;

    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var parser = new Parser(s =>
            {
                s.AutoHelp = true;
                s.AutoVersion = true;
                s.CaseInsensitiveEnumValues = true;
                s.CaseSensitive = false;
                s.HelpWriter = Console.Out;
                s.IgnoreUnknownArguments = false;
                s.MaximumDisplayWidth = 100;
            });

            var exitCode = await parser.ParseArguments<RunOptions, UpdateOptions>(args)
                .MapResult(
                    async (RunOptions o) => await RunCommand.ExecuteAsync(o),
                    async (UpdateOptions o) => await UpdateCommand.ExecuteAsync(o),
                    errs => Task.FromResult(ExitCode.Failure));

            return (int)exitCode;
        }
    }
}
