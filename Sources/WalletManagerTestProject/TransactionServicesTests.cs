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
            var transactionSerializer = new WalletManagerServices.Serializer.TransactionSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);

            // Act
            var transactions = transactionServices.GetTransactions();

            // Assert
            Assert.True(transactions.Any());
        }
    }
}
