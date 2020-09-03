using System.Collections.Generic;

namespace WalletManagerServices.Category
{
    public interface ICategoryServices
    {
        bool CreateCategories(List<string> categories, string csvPath);
    }
}
