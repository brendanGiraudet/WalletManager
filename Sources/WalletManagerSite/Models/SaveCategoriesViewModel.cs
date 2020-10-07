
using System.ComponentModel.DataAnnotations;

namespace WalletManagerSite.Models
{
    public class SaveCategoriesViewModel
    {
        [Required(ErrorMessage = "Choose a filename of your csv file"),
        FileExtensions(Extensions = "csv", ErrorMessage = "Use csv extension")]
        public string FileName { get; set; }

    }
}
