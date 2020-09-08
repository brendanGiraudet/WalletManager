using WalletManagerDAL.Serializer;
using Xunit;
using System.Collections.Generic;
using System.IO;
using Bogus;
using System.Linq;

namespace WalletManagerTestProject.Serializer
{
    public class CategorySerializerTests
    {
        const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CategoriesCsv\";

        readonly ISerializer<string> _serializer;

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

            // Act
            var categories = _serializer.Deserialize(csvFilePath);

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
            var faker = new Faker();
            var categories = new List<string>
            {
                faker.Random.String2(2),
                faker.Random.String2(2)
            };

            // Act
            var serializeResponse = _serializer.Serialize(categories, csvFilePath);

            // Assert
            Assert.True(serializeResponse);
        }
        #endregion
    }
}
