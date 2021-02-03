using System;
using System.Collections.Generic;
using System.Linq;
using WalletManagerDAL.Serializer;
using WalletManagerServices.Transaction;
using Xunit;

namespace WalletManagerTestProject.Service
{
    public class TransactionServicesTests
    {
        const string csvBasePath = @"/home/runner/work/WalletManager/WalletManager/Sources\WalletManagerTestProject\CSV\";
        //const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\";
        readonly ITransactionServices _transactionServices;

        public TransactionServicesTests()
        {
            _transactionServices = new TransactionServices();
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
                OperationDate = new DateTime(2020, 03, 31),
                Label = "Label1",
                Reference = "ref1",
                Amount = 10,
                Category = new WalletManagerDTO.Category { Name = "Courses" }
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
                Category = new WalletManagerDTO.Category { Name = "Courses" }
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
            const decimal expectedGroupedPaypalAmount = -25;

            // Act
            var transactions = _transactionServices.GetGroupedTransactionsByLabel();
            var paypalTransaction = transactions.FirstOrDefault(t => t.Label.Contains("PAYPAL         750800"));

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
            const decimal expectedGroupedNAAmount = -2000;
            var debitTransactions = _transactionServices.GetDebitTransactions();

            // Act
            var transactions = _transactionServices.GetGroupedTransactionsByCategory(debitTransactions);
            var NATransaction = transactions.FirstOrDefault(t => t.Category.Name.Equals("Courses"));

            // Assert
            Assert.Equal(expectedGroupedNAAmount, NATransaction.Amount);
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
                    Category = new WalletManagerDTO.Category { Name = "Courses" },
                    Reference = "ref1",
                    OperationDate = DateTime.Now,
                    Compte = "Compte1",
                    Label = "Label1"
                },
                new WalletManagerDTO.Transaction
                {
                    Amount = 20,
                    Category = new WalletManagerDTO.Category { Name = "Internet" },
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
                    Category = new WalletManagerDTO.Category { Name = "Courses" },
                    Reference = "ref1",
                    OperationDate = DateTime.Now,
                    Compte = "Compte1",
                    Label = "Label1"
                },
                new WalletManagerDTO.Transaction
                {
                    Amount = 20,
                    Category = new WalletManagerDTO.Category { Name = "Internet" },
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

        [Fact]
        public void ShouldOneTransactionListWhenFusionTwoCsv()
        {
            // Arrange
            var csvPath = csvBasePath + "deserialize.csv";
            _transactionServices.LoadTransactions(csvPath);
            const decimal expectedGroupedCourseAmount = -4000;
            var firstTransactionListToFusion = _transactionServices.GetTransactions();
            var secondTransactionListToFusion = _transactionServices.GetTransactions();
            var categoryName = "Courses";

            // Act
            var fusionedTransactions = _transactionServices.FusionTransactions(firstTransactionListToFusion, secondTransactionListToFusion);
            var coursesTransactions = fusionedTransactions.Where(t => t.Category.Name.Equals(categoryName));
            var courseTransactionsAmount = coursesTransactions.Sum(t => t.Amount);

            // Assert
            Assert.Equal(expectedGroupedCourseAmount, courseTransactionsAmount);
        }
    }
}
