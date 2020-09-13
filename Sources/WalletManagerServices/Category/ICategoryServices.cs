using System.Collections.Generic;

namespace WalletManagerServices.Category
{
    public interface ICategoryServices
    {
        bool SaveCategories(IEnumerable<WalletManagerDTO.Category> categories, string csvPath);

        IEnumerable<WalletManagerDTO.Category> GetCategories(string filePath);
    }
}
