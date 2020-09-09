using WalletManagerDAL.Serializer;
using WalletManagerDTO;
using WalletManagerServices.Category;
using WalletManagerTestProject.Utils;
using Xunit;

namespace WalletManagerTestProject.Service
{
    public class CategoryServicesTests
    {
        const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CategoriesCsv\";
        readonly ICategoryServices _categoryServices;

        public CategoryServicesTests()
        {
            ISerializer<Category> serializer = new CategorySerializer();
            _categoryServices = new CategoryServices(serializer);
        }

        [Fact]
        public void ShouldHaveTrueWhenSaveCategories()
        {
            // Arrange
            var csvPath = csvBasePath + "saveCategories.csv";
            var fakeCategories = FakerUtils.CategoryFaker.Generate(2);

            // Act
            var isCreated = _categoryServices.SaveCategories(fakeCategories, csvPath);

            // Assert
            Assert.True(isCreated);
        }
    }
}
