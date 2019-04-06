namespace FootSim.Commands
{
    using System.Threading.Tasks;

    public interface ICommand
    {
        Task<ExitCode> ExecuteAsync();
    }
}