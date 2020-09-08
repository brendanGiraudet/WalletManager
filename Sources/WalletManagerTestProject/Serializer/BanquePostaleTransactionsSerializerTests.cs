using WalletManagerDAL.Serializer;
using Xunit;
using System.IO;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerTestProject.Serializer
{
    public class BanquePostaleTransactionsSerializerTests
    {
        const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\BanquePostaleTransactionsCsv\";

        readonly ISerializer<Transaction> _serializer;

        public BanquePostaleTransactionsSerializerTests()
        {
            _serializer = new BanquePostaleTransactionsSerializer();
        }

        #region Deserialize
        [Fact]
        public void ShouldDeserializeListOfTransaction()
        {
            // Arrange
            var csvFilePath = Path.Combine(csvBasePath, "deserializeTransactions.csv");
            var lines = File.ReadAllLines(csvFilePath);

            // Act
            var transactions = _serializer.Deserialize(lines);

            // Assert
            Assert.True(transactions.Any());
        }
        #endregion
    }
}
