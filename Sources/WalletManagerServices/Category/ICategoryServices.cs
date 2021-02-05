using System.Collections.Generic;
using System.Threading.Tasks;

namespace WalletManagerServices.Category
{
    public interface ICategoryServices
    {
        bool SaveCategories(IEnumerable<WalletManagerDTO.Category> categories, string csvPath);

        Task<IEnumerable<WalletManagerDTO.Category>> GetCategories(string filePath);
        
        WalletManagerDTO.Category GetCategory(string categoryName);

        void AddCategory(WalletManagerDTO.Category category);
        
        void Delete(string categoryName);
        
        bool SaveCategories(string csvPath);
    }
}
