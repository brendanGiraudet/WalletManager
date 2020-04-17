
using System.ComponentModel.DataAnnotations;

namespace WalletManagerSite.Models
{
    public class SaveTransactionsViewModel
    {
        [Required(ErrorMessage = "Choose a filename of your csv file"),
        FileExtensions(Extensions = "csv", ErrorMessage = "Error")]
        public string FileName { get; set; }

    }
}
