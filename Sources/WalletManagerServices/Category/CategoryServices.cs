using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WalletManagerServices.Category
{
    public class CategoryServices : ICategoryServices
    {
        readonly WalletManagerDAL.Serializer.ISerializer<WalletManagerDTO.Category> _serializer;
        private IEnumerable<WalletManagerDTO.Category> _categories = Enumerable.Empty<WalletManagerDTO.Category>();

        public CategoryServices(WalletManagerDAL.Serializer.ISerializer<WalletManagerDTO.Category> serializer)
        {
            _serializer = serializer;
        }

        public IEnumerable<WalletManagerDTO.Category> GetCategories(string filePath)
        {
            if(!_categories.Any())
            {
                var lines = File.ReadAllLines(filePath);
                _categories = _serializer.Deserialize(lines);
            }
            
            return _categories;
        }

        public bool SaveCategories(string csvPath)
        {
            return SaveCategories(_categories, csvPath);
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

        public WalletManagerDTO.Category GetCategory(string categoryName)
        {
            return _categories.FirstOrDefault(c => c.Name.Equals(categoryName));
        }

        public void AddCategory(WalletManagerDTO.Category category)
        {
            var categoryList = _categories.ToList();
            categoryList.Add(category);
            _categories = categoryList;
        }

        public void Delete(string categoryName)
        {
            _categories = _categories.Where(t => t.Name != categoryName);
        }
    }
}
