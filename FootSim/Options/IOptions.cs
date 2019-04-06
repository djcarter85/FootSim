namespace FootSim.Options
{
    using FootSim.Commands;

    public interface IOptions
    {
        ICommand CreateCommand();
    }
}