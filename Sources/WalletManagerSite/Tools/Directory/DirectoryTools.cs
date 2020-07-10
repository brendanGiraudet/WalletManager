using Microsoft.Extensions.Configuration;
using System.IO;

namespace WalletManagerSite.Tools.Directory
{
    public static class DirectoryTools
    {
        public static string GetCsvDirectoryPath(IConfiguration configuration)
        {
            var directoryName = configuration.GetValue<string>("CsvDirectoryName");
            var directoryPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), directoryName);
            CreateDirectoryIfNotExist(directoryPath);
            return directoryPath;
        }

        public static void CreateDirectoryIfNotExist(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
        }

        public static string[] GetCsvFiles(string directoryPath)
        {
            var csvFilesName = System.IO.Directory.GetFiles(directoryPath, "*.csv");
            return csvFilesName;
        }
    }
}
