using Bogus;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WalletManagerDAL.File;
using WalletManagerDAL.Serializer;
using WalletManagerDTO;
using WalletManagerTestProject.Utils;
using Xunit;

namespace WalletManagerTestProject.Serializer
{
    public class CategorySerializerTests
    {
        private IEnumerable<string> _contentFile = Enumerable.Empty<string>();

        readonly ISerializer<Category> _serializer;

        public CategorySerializerTests()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(f => f.Write(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(FakerUtils.BoolResponseFaker.Generate())
                .Verifiable();

            _serializer = new CategorySerializer(fileServiceMock.Object);
            _contentFile = _contentFile.Append("Name;CreationDate");
            _contentFile = _contentFile.Append("Course;16-Jun-20 12:36:14 AM");
            _contentFile = _contentFile.Append("Internet;16-Jun-20 12:36:14 AM");
            _contentFile = _contentFile.Append("Téléphone;16-Jun-20 12:36:14 AM");
        }

        #region Deserialize
        [Fact]
        public void ShouldDeserializeListOfCategory()
        {
            // Arrange

            // Act
            var categories = _serializer.Deserialize(_contentFile);

            // Assert
            Assert.True(categories.Any());
        }
        #endregion

        #region Serialize
        [Fact]
        public async Task ShouldHavetrueWhenSerializeListOfCategory()
        {
            // Arrange
            var faker = new Faker();
            var csvPath = faker.Random.String2(2);
            var categories = FakerUtils.CategoryFaker.Generate(2);

            // Act
            var serializeResponse = await _serializer.Serialize(categories, csvPath);

            // Assert
            Assert.True(serializeResponse);
        }
        #endregion
    }
}
