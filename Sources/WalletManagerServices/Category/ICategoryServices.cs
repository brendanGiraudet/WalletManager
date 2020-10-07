using System.Collections.Generic;

namespace WalletManagerServices.Category
{
    public interface ICategoryServices
    {
        bool SaveCategories(IEnumerable<WalletManagerDTO.Category> categories, string csvPath);

        IEnumerable<WalletManagerDTO.Category> GetCategories(string filePath);
        
        WalletManagerDTO.Category GetCategory(string categoryName);

        void AddCategory(WalletManagerDTO.Category category);
        
        void Delete(string categoryName);
        
        bool SaveCategories(string csvPath);
    }
}
