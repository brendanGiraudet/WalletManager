using System.Linq;
using Xunit;

namespace WalletManagerTestProject
{
    public class TransactionServicesTests
    {
        [Fact]
        public void ShouldLoadTransactionsWhenIPutCsvDataFile()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserialize.csv";

            // Act
            transactionServices.LoadTransactions(csvPath);

            // Assert
            Assert.True(true); // because no exceptions are thrown
        }

        [Fact]
        public void ShouldHaveListOfTransactionWhenIGetTransactions()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserialize.csv";
            transactionServices.LoadTransactions(csvPath);

            // Act
            var transactions = transactionServices.GetTransactions();

            // Assert
            Assert.True(transactions.Any());
        }

        [Fact]
        public void ShouldHaveAtransactionWhenIPutAGoodReference()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            const string expectedReference = "2V7926X";
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserialize.csv";
            transactionServices.LoadTransactions(csvPath);

            // Act
            var transaction = transactionServices.GetTransaction(expectedReference);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(transaction.Reference, expectedReference);
        }
    }
}
