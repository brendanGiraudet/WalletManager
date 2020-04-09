using Configuration;
using System.Linq;
using Xunit;

namespace WalletManagerTestProject
{
    public class TransactionServicesTests
    {
        [Fact]
        public void ShouldHaveListOfTransactionWhenIGetTransactions()
        {
            // Arrange
            var configurator = new Configurator();
            var transactionSerializer = new WalletManagerServices.Serializer.TransactionSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer, configurator);

            // Act
            var transactions = transactionServices.GetTransactions();

            // Assert
            Assert.True(transactions.Any());
        }

        [Fact]
        public void ShouldHaveAtransactionWhenIPutAGoodReference()
        {
            // Arrange
            var configurator = new Configurator();
            var transactionSerializer = new WalletManagerServices.Serializer.TransactionSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer, configurator);
            const string expectedReference = "2V7926X";

            // Act
            var transaction = transactionServices.GetTransaction(expectedReference);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(transaction.Reference, expectedReference);

        }
    }
}
