
using System.ComponentModel.DataAnnotations;

namespace WalletManagerSite.Models
{
    public class SaveTransactionsViewModel
    {
        [Required(ErrorMessage = "Choose a filename of your csv file"),
        FileExtensions(Extensions = "csv", ErrorMessage = "Use csv extension")]
        public string FileName { get; set; }

    }
}
