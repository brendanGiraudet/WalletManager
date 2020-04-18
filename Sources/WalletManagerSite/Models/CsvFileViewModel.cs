using System;

namespace WalletManagerSite.Models
{
    public class CsvFileViewModel
    {
        public string FullPath { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
