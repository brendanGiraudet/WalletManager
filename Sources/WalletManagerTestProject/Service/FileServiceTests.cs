using Bogus;
using System.Threading.Tasks;
using WalletManagerServices.File;
using Xunit;

namespace WalletManagerTestProject.Service
{
    public class FileServiceTests
    {
        private readonly IFileService _fileService;
        public FileServiceTests()
        {
            _fileService = new FileService();
        }

        #region Write
        [Fact]
        public async Task ShouldHaveAnErrorWhenWriteAFileWithEmptyFilePath()
        {
            // Arrange
            var faker = new Faker();
            var fakeFilePath = "";
            var fakeContentFile = faker.Random.String2(2);

            // Act
            var writeResponse = await _fileService.Write(fakeFilePath, fakeContentFile);

            // Assert
            Assert.True(writeResponse.HasError);
        }
        [Fact]
        public async Task ShouldHaveAnErrorWhenWriteAFileWithEmptyContent()
        {
            // Arrange
            var faker = new Faker();
            var fakeFilePath = faker.Random.String2(2);
            var fakeContentFile = "";

            // Act
            var writeResponse = await _fileService.Write(fakeFilePath, fakeContentFile);

            // Assert
            Assert.True(writeResponse.HasError);
        }
        #endregion
        
        #region Read
        [Fact]
        public async Task ShouldHaveAnErrorWhenReadAFileWithEmptyFilePath()
        {
            // Arrange
            var fakeFilePath = string.Empty;

            // Act
            var readResponse = await _fileService.Read(fakeFilePath);

            // Assert
            Assert.True(readResponse.HasError);
        }
        #endregion
    }
}
