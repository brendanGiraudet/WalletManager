using Bogus;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManagerDAL.File;
using WalletManagerDAL.Serializer;
using WalletManagerDTO;
using WalletManagerTestProject.Utils;
using Xunit;

namespace WalletManagerTestProject.Serializer
{
    public class TransactionsSerializerTests
    {
        readonly ISerializer<Transaction> _serializer;
        private IEnumerable<string> _contentFile = Enumerable.Empty<string>();

        public TransactionsSerializerTests()
        {
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock
                .Setup(f => f.Write(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(FakerUtils.BoolResponseFaker.Generate())
                .Verifiable();

            _serializer = new TransactionsSerializer(fileServiceMock.Object);

            _contentFile = _contentFile.Append("Compte;Date opération;Libellé;Référence;Montant;Categorie");
            _contentFile = _contentFile.Append("mf;17-Oct-19 5:14:44 AM;kf;vp;-1000;Courses;");
            _contentFile = _contentFile.Append("xx;09-Apr-20 4:51:01 PM;hm;fy;-1000;Courses;");
        }

        #region Deserialize
        [Fact]
        public void ShouldDeserializeListOfTransaction()
        {
            // Arrange

            // Act
            var transactions = _serializer.Deserialize(_contentFile);

            // Assert
            Assert.True(transactions.Any());
        }
        #endregion

        #region Serialize
        [Fact]
        public async Task ShouldHavetrueWhenSerializeListOfTransaction()
        {
            // Arrange
            var faker = new Faker();
            var csvPath = faker.Random.String2(2);
            var transactions = FakerUtils.GetTransactionDtoFaker.Generate(2);

            // Act
            var serializeResponse = await _serializer.Serialize(transactions, csvPath);

            // Assert
            Assert.True(serializeResponse);
        }
        #endregion
    }
}
