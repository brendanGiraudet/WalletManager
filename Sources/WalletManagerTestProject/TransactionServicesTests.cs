using System;
using System.Collections.Generic;
using System.Linq;
using WalletManagerDAL.Serializer;
using WalletManagerServices.Transaction;
using Xunit;

namespace WalletManagerTestProject
{
    public class TransactionServicesTests
    {
        const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\";
        readonly ITransactionServices _transactionServices;

        public TransactionServicesTests()
        {
            ISerializer serializer = new CsvSerializer();
            _transactionServices = new TransactionServices(serializer);
        }

        [Fact]
        public void ShouldLoadTransactionsWhenIPutCsvDataFile()
        {
            // Arrange
            var csvPath = csvBasePath + "deserialize.csv";

            // Act
            _transactionServices.LoadTransactions(csvPath);

            // Assert
            Assert.True(true); // because no exceptions are thrown
        }

        [Fact]
        public void ShouldHaveListOfTransactionWhenIGetTransactions()
        {
            // Arrange
            var csvPath = csvBasePath + "deserialize.csv";
            _transactionServices.LoadTransactions(csvPath);

            // Act
            var transactions = _transactionServices.GetTransactions();

            // Assert
            Assert.True(transactions.Any());
        }

        [Fact]
        public void ShouldHaveAtransactionWhenIPutAGoodReference()
        {
            // Arrange
            const string expectedReference = "475QGS0";
            var csvPath = csvBasePath + "deserialize.csv";
            _transactionServices.LoadTransactions(csvPath);

            // Act
            var transaction = _transactionServices.GetTransaction(expectedReference);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(transaction.Reference, expectedReference);
        }

        [Fact]
        public void ShouldUpdateCategoryTransaction()
        {
            // Arrange
            var csvPath = csvBasePath + "save.csv";
            _transactionServices.LoadTransactions(csvPath);
            var updatedTransaction = new WalletManagerDTO.Transaction
            {
                Compte = "Compte1",
                OperationDate = new System.DateTime(2020, 03, 31),
                Label = "Label1",
                Reference = "ref1",
                Amount = 10,
                Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses
            };

            // Act
            _transactionServices.UpdateTransaction(updatedTransaction);
            var transaction = _transactionServices.GetTransaction(updatedTransaction.Reference);

            // Assert
            Assert.Equal(updatedTransaction.Category, transaction.Category);
        }

        [Fact]
        public void ShouldThrownExceptionWhenUpdateCategoryTransaction()
        {
            // Arrange
            var updatedTransaction = new WalletManagerDTO.Transaction
            {
                Reference = "doesntExist",
                Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses
            };

            // Act
            Action updateTransactionAction = () => _transactionServices.UpdateTransaction(updatedTransaction);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.TransactionServiceException>(updateTransactionAction);
        }

        [Fact]
        public void ShouldHaveListOfRegroupedTransactionWhenIGetGroupedTransactions()
        {
            // Arrange
            var csvPath = csvBasePath + "deserialize.csv";
            _transactionServices.LoadTransactions(csvPath);
            const double expectedGroupedPaypalAmount = -25;

            // Act
            var transactions = _transactionServices.GetGroupedTransactionsByLabel();
            var paypalTransaction = transactions.Find(t => t.Label.Contains("PAYPAL         750800"));

            // Assert
            Assert.Equal(expectedGroupedPaypalAmount, Math.Round(paypalTransaction.Amount, 2));
        }

        [Fact]
        public void ShouldHaveOnlyDebitTransactionsWhenGetDebitTransactions()
        {
            // Arrange
            var csvPath = csvBasePath + "deserialize.csv";
            _transactionServices.LoadTransactions(csvPath);

            // Act
            var debitTransactions = _transactionServices.GetDebitTransactions();

            // Assert
            Assert.DoesNotContain(debitTransactions, t => t.Amount >= 0);
        }

        [Fact]
        public void ShouldHaveListOfRegroupedTransactionByCategory()
        {
            // Arrange
            var csvPath = csvBasePath + "deserialize.csv";
            _transactionServices.LoadTransactions(csvPath);
            const double expectedGroupedNAAmount = -2000;

            // Act
            var transactions = _transactionServices.GetGroupedTransactionsByCategory(_transactionServices.GetDebitTransactions());
            var NATransaction = transactions.Find(t => t.Category.Equals(WalletManagerDTO.Enumerations.TransactionCategory.Courses));

            // Assert
            Assert.Equal(expectedGroupedNAAmount, Math.Round(NATransaction.Amount, 2));
        }

        [Fact]
        public void ShouldSaveTransactionsIntoCsvFile()
        {
            // Arrange
            var csvPath = csvBasePath + "save.csv";
            var transactionsToSave = new List<WalletManagerDTO.Transaction>
            {
                new WalletManagerDTO.Transaction
                {
                    Amount = 10,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses,
                    Reference = "ref1",
                    OperationDate = DateTime.Now,
                    Compte = "Compte1",
                    Label = "Label1"
                },
                new WalletManagerDTO.Transaction
                {
                    Amount = 20,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Internet,
                    Reference = "ref2",
                    OperationDate = DateTime.Now,
                    Compte = "Compte2",
                    Label = "Label2"
                }
            };

            // Act
            _transactionServices.SaveTransactionsIntoCsvFile(csvPath, transactionsToSave);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void ShouldThrownExceptionWhenITryToSaveTransactionsIntoCsvFileWithEmptyTransactionList()
        {
            // Arrange
            var csvPath = csvBasePath + "save.csv";
            var transactionsToSave = new List<WalletManagerDTO.Transaction>();

            // Act
            Action saveTransactionsIntoCsvFileAction = () => _transactionServices.SaveTransactionsIntoCsvFile(csvPath, transactionsToSave);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.TransactionServiceException>(saveTransactionsIntoCsvFileAction);
        }

        [Fact]
        public void ShouldThrownExceptionWhenITryToSaveTransactionsIntoCsvFileWithEmptyCsvPath()
        {
            // Arrange
            var csvPath = "";
            var transactionsToSave = new List<WalletManagerDTO.Transaction>
            {
                new WalletManagerDTO.Transaction
                {
                    Amount = 10,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses,
                    Reference = "ref1",
                    OperationDate = DateTime.Now,
                    Compte = "Compte1",
                    Label = "Label1"
                },
                new WalletManagerDTO.Transaction
                {
                    Amount = 20,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Internet,
                    Reference = "ref2",
                    OperationDate = DateTime.Now,
                    Compte = "Compte2",
                    Label = "Label2"
                }
            };

            // Act
            Action saveTransactionsIntoCsvFileAction = () => _transactionServices.SaveTransactionsIntoCsvFile(csvPath, transactionsToSave);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.TransactionServiceException>(saveTransactionsIntoCsvFileAction);
        }

        [Fact]
        public void ShouldDeleteTransaction()
        {
            // Arrange
            var filePath = csvBasePath + "save.csv";
            var expectedReference = "ref2";
            _transactionServices.LoadTransactions(filePath);

            // Act
            _transactionServices.Delete(expectedReference);
            var deletedTransaction = _transactionServices.GetTransaction(expectedReference);

            // Assert
            Assert.Null(deletedTransaction);
        }
    }
}
