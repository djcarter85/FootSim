namespace FootSim.Core
{
    using System.IO;
    using System.Reflection;

    public static class CsvEmbeddedResources
    {
        public static Stream GetStream(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream($"FootSim.Core.{fileName}");
        }
    }
}