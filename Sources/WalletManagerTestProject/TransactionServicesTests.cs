using System;
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

        [Fact]
        public void ShouldThrownExceptionWhenUpdateCategoryTransaction()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            var updatedTransaction = new WalletManagerDTO.Transaction
            {
                Reference = "doesntExist",
                Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses
            };

            // Act
            Action updateTransactionAction = () => transactionServices.UpdateTransaction(updatedTransaction);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.TransactionServiceException>(updateTransactionAction);
        }

        [Fact]
        public void ShouldHaveListOfRegroupedTransactionWhenIGetGroupedTransactions()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\regroup.csv";
            transactionServices.LoadTransactions(csvPath);
            const double expectedGroupedPaypalAmount = -288.69;

            // Act
            var transactions = transactionServices.GetGroupedTransactionsByLabel();
            var paypalTransaction = transactions.Find(t => t.Label.Contains("PAYPAL         750800"));

            // Assert
            Assert.Equal(expectedGroupedPaypalAmount, Math.Round(paypalTransaction.Amount, 2));
        }

        [Fact]
        public void ShouldHaveOnlyDebitTransactionsWhenGetDebitTransactions()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserialize.csv";
            transactionServices.LoadTransactions(csvPath);

            // Act
            var debitTransactions = transactionServices.GetDebitTransactions();

            // Assert
            Assert.DoesNotContain(debitTransactions, t => t.Amount >= 0);
        }

        [Fact]
        public void ShouldHaveListOfRegroupedTransactionByCategory()
        {
            // Arrange
            var transactionSerializer = new WalletManagerDAL.Serializer.CsvSerializer();
            var transactionServices = new WalletManagerServices.Transaction.TransactionServices(transactionSerializer);
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\regroupByCategory.csv";
            transactionServices.LoadTransactions(csvPath);
            const double expectedGroupedNAAmount = -2000;

            // Act
            var transactions = transactionServices.GetGroupedTransactionsByCategory(transactionServices.GetDebitTransactions());
            var NATransaction = transactions.Find(t => t.Category.Equals(WalletManagerDTO.Enumerations.TransactionCategory.NA));

            // Assert
            Assert.Equal(expectedGroupedNAAmount, Math.Round(NATransaction.Amount, 2));
        }
    }
}
