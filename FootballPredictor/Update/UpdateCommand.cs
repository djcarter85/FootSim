namespace FootballPredictor.Update
{
    using System.Threading.Tasks;
    using FootballPredictor.Core;

    public class UpdateCommand
    {
        public static async Task<int> RunAsync(UpdateOptions options)
        {
            var repository = new Repository(Constants.CsvFilePath, Constants.Url, null);

            await repository.RefreshFromWebAsync();

            return 0;
        }
    }
}