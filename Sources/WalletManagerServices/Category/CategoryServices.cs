using System;
using System.Collections.Generic;

namespace WalletManagerServices.Category
{
    public class CategoryServices : ICategoryServices
    {
        readonly WalletManagerDAL.Serializer.ISerializer _serializer;

        public CategoryServices(WalletManagerDAL.Serializer.ISerializer serializer)
        {
            _serializer = serializer;
        }

        public bool CreateCategories(List<string> categories, string csvPath)
        {
            try
            {
                _serializer.Serialize(categories, csvPath);
                return true;
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.TransactionServiceException($"Impossible to save transactions due to {ex.Message}", ex);
            }
        }
    }
}
