namespace FootSim
{
    using System;
    using System.Threading.Tasks;
    using CommandLine;
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

            var parserResult = parser.ParseArguments<RunOptions, UpdateOptions>(args);

            var exitCode = await ExecuteCommand(parserResult);

            return (int)exitCode;
        }

        private static async Task<ExitCode> ExecuteCommand(ParserResult<object> parserResult)
        {
            var parsed = parserResult as Parsed<object>;

            if (parsed?.Value is IOptions options)
            {
                return await options.CreateCommand().ExecuteAsync();
            }

            return ExitCode.Failure;
        }
    }
}
