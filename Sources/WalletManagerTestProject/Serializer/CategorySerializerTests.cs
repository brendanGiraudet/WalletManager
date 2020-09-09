using WalletManagerDAL.Serializer;
using Xunit;
using System.Collections.Generic;
using System.IO;
using Bogus;
using System.Linq;
using WalletManagerDTO;
using WalletManagerTestProject.Utils;

namespace WalletManagerTestProject.Serializer
{
    public class CategorySerializerTests
    {
        const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CategoriesCsv\";

        readonly ISerializer<Category> _serializer;

        public CategorySerializerTests()
        {
            _serializer = new CategorySerializer();
        }

        #region Deserialize
        [Fact]
        public void ShouldDeserializeListOfCategory()
        {
            // Arrange
            var csvFilePath = Path.Combine(csvBasePath, "deserializeCategories.csv");
            var lines = File.ReadAllLines(csvFilePath);

            // Act
            var categories = _serializer.Deserialize(lines);

            // Assert
            Assert.True(categories.Any());
        }
        #endregion

        #region Serialize
        [Fact]
        public void ShouldHavetrueWhenSerializeListOfCategory()
        {
            // Arrange
            var csvFilePath = Path.Combine(csvBasePath, "serializeCategories.csv");
            var categories = FakerUtils.CategoryFaker.Generate(2);

            // Act
            var serializeResponse = _serializer.Serialize(categories, csvFilePath);

            // Assert
            Assert.True(serializeResponse);
        }
        #endregion
    }
}
