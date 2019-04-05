namespace FootSim.Update
{
    using System.Threading.Tasks;
    using FootSim.Core;

    public class UpdateCommand
    {
        public static async Task<int> RunAsync(UpdateOptions options)
        {
            var repository = new Repository(Constants.CsvFilePath, Constants.Url);

            await repository.RefreshFromWebAsync();

            return 0;
        }
    }
}