using Microsoft.Extensions.Configuration;

namespace WalletManagerSite.Tools.Directory
{
    public static class DirectoryTools
    {
        public static string GetCsvDirectoryPath(IConfiguration configuration)
        {
            var directoryPath = configuration.GetValue<string>("CsvDirectoryPath");
            CreateDirectoryIfNotExist(directoryPath);
            return directoryPath;
        }
        
        public static string GetCategoryCsvFilePath(IConfiguration configuration)
        {
            var categoryCsvFilePath = configuration.GetValue<string>("CategoryCsvFilePath");
            return categoryCsvFilePath;
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
