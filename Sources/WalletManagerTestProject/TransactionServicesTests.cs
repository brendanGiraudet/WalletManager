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

        [Fact]
        public void ShouldUpdateCategoryTransaction()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\update.csv";
            transactionServices.LoadTransactions(csvPath);
            var updatedTransaction = new WalletManagerDTO.Transaction
            {
                Compte = "31419185918",
                ComptabilisationDate = new System.DateTime(2020, 03, 31),
                OperationDate = new System.DateTime(2020, 03, 31),
                Label = "VIR M BRENDAN GIRAUDET Virement vers BRENDAN GIRAUDET",
                Reference = "6681107",
                ValueDate = new System.DateTime(2020, 03, 31),
                Amount = -1000,
                Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses
            };

            // Act
            transactionServices.UpdateTransaction(updatedTransaction);
            var transaction = transactionServices.GetTransaction(updatedTransaction.Reference);

            // Assert
            Assert.Equal(updatedTransaction.Category, transaction.Category);
        }
    }
}
