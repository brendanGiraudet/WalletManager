using Bogus;
using System.Collections.Generic;
using WalletManagerDAL.Serializer;
using WalletManagerServices.Category;
using Xunit;

namespace WalletManagerTestProject
{
    public class CategoryServicesTests
    {
        const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CategoriesCsv\";
        readonly ICategoryServices _categoryServices;

        public CategoryServicesTests()
        {
            ISerializer serializer = new CsvSerializer();
            _categoryServices = new CategoryServices(serializer);
        }

        [Fact]
        public void ShouldHaveTrueWhenCreateCategory()
        {
            // Arrange
            var csvPath = csvBasePath + "createCategories.csv";
            var faker = new Faker();
            var fakeCategories = new List<string>
            {
                faker.Random.String2(2),
                faker.Random.String2(2)
            };

            // Act
            var isCreated = _categoryServices.CreateCategories(fakeCategories, csvPath);

            // Assert
            Assert.True(isCreated);
        }
    }
}
