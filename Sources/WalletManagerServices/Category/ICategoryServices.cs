using System.Collections.Generic;

namespace WalletManagerServices.Category
{
    public interface ICategoryServices
    {
        bool SaveCategories(List<string> categories, string csvPath);
    }
}
