using Bogus;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManagerDAL.File;
using WalletManagerDAL.Serializer;
using WalletManagerDTO;
using WalletManagerServices.Category;
using WalletManagerTestProject.Utils;
using Xunit;

namespace WalletManagerTestProject.Service
{
    public class CategoryServicesTests
    {
        readonly ICategoryServices _categoryServices;

        public CategoryServicesTests()
        {
            var faker = new Faker();
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(f => f.Read(It.IsAny<string>()))
                .ReturnsAsync(FakerUtils.StringResponseFaker.Generate())
                .Verifiable();

            var categorySerializerMock = new Mock<ISerializer<Category>>();
            categorySerializerMock
                .Setup(c => c.Deserialize(It.IsAny<IEnumerable<string>>()))
                .Returns(FakerUtils.CategoryFaker.Generate(2))
                .Verifiable();
            categorySerializerMock
                .Setup(c => c.Serialize(It.IsAny<IEnumerable<Category>>(), It.IsAny<string>()))
                .ReturnsAsync(true)
                .Verifiable();

            _categoryServices = new CategoryServices(categorySerializerMock.Object, fileServiceMock.Object);
        }

        [Fact]
        public void ShouldHaveTrueWhenSaveCategories()
        {
            // Arrange
            var faker = new Faker();
            var csvPath = faker.Random.String2(2);
            var fakeCategories = FakerUtils.CategoryFaker.Generate(2);

            // Act
            var isCreated = _categoryServices.SaveCategories(fakeCategories, csvPath);

            // Assert
            Assert.True(isCreated);
        }

        [Fact]
        public async Task ShouldHaveListOfCategory()
        {
            // Arrange
            var faker = new Faker();
            var filePath = faker.Random.String2(2);

            // Act
            var categories = await _categoryServices.GetCategories(filePath);

            // Assert
            Assert.True(categories.Any());
        }
    }
}
