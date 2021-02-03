using WalletManagerDAL.Serializer;
using Xunit;
using System.IO;
using System.Linq;
using WalletManagerDTO;
using WalletManagerTestProject.Utils;

namespace WalletManagerTestProject.Serializer
{
    public class TransactionsSerializerTests
    {
        const string csvBasePath = @"/home/runner/work/WalletManager/WalletManager/Sources\WalletManagerTestProject\TransactionsCsv\";
        //const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\TransactionsCsv\";

        readonly ISerializer<Transaction> _serializer;

        public TransactionsSerializerTests()
        {
            _serializer = new TransactionsSerializer();
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

        #region Serialize
        [Fact]
        public void ShouldHavetrueWhenSerializeListOfTransaction()
        {
            // Arrange
            var csvFilePath = Path.Combine(csvBasePath, "serializeTransactions.csv");
            var transactions = FakerUtils.GetTransactionDtoFaker.Generate(2);

            // Act
            var serializeResponse = _serializer.Serialize(transactions, csvFilePath);

            // Assert
            Assert.True(serializeResponse);
        }
        #endregion
    }
}
