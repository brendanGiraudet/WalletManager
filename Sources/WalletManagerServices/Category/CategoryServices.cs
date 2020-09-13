using System;
using System.Collections.Generic;
using System.IO;

namespace WalletManagerServices.Category
{
    public class CategoryServices : ICategoryServices
    {
        readonly WalletManagerDAL.Serializer.ISerializer<WalletManagerDTO.Category> _serializer;

        public CategoryServices(WalletManagerDAL.Serializer.ISerializer<WalletManagerDTO.Category> serializer)
        {
            _serializer = serializer;
        }

        public IEnumerable<WalletManagerDTO.Category> GetCategories(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            return _serializer.Deserialize(lines);
        }

        public bool SaveCategories(IEnumerable<WalletManagerDTO.Category> categories, string csvPath)
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
