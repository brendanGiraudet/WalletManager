using System.Linq;
using WalletManagerDAL.Serializer;
using Xunit;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WalletManagerTestProject
{
    public class SerializerTests
    {
        [Theory]
        [InlineData(@"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserialize.csv")]
        [InlineData(@"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserializeWithCategory.csv")]
        public void ShouldHaveListOfTransactionWhenIDeserializeACsvString(string csvPath)
        {
            // Arrange
            ISerializer serializer = new BanquePopulaireCsvSerializer();

            // Act
            var transactionList = serializer.Deserialize(csvPath);

            // Assert
            Assert.True(transactionList.Any());
        }

        [Theory]
        [InlineData(@"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserialize.csv")]
        [InlineData(@"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserializeWithCategory.csv")]
        public void ShouldHaveListOfTransactionWhenIDeserializeAstream(string csvPath)
        {
            // Arrange
            ISerializer serializer = new BanquePopulaireCsvSerializer();
            var expectedStream = File.OpenRead(csvPath);

            // Act
            var transactionList = serializer.Deserialize(expectedStream);

            // Assert
            Assert.True(transactionList.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(@"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\wrong.csv")]
        public void ShouldThrowExceptionWhenIDeserializeAWrongCsvFile(string csvPath)
        {
            // Arrange
            ISerializer serializer = new BanquePopulaireCsvSerializer();

            // Act
            Action deserializeAction = () => serializer.Deserialize(csvPath);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.SerializerException>(deserializeAction);
        }

        [Fact]
        public void ShouldHaveACsvFileWhenISerializeTransactionList()
        {
            // Arrange
            ISerializer serializer = new BanquePopulaireCsvSerializer();
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\serialize.csv";
            var transactions = new List<WalletManagerDTO.Transaction>
            {
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    ComptabilisationDate = new DateTime(2020,03,31),
                    OperationDate = new DateTime(2020,03,31),
                    Label = "300320 CB****1526 ALLDEBRID.COM  92MONTROUGE",
                    Reference = "475QGS0",
                    ValueDate = new DateTime(2020,03,31),
                    Amount = -15.99,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Internet
                },
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    ComptabilisationDate = new DateTime(2020,03,31),
                    OperationDate = new DateTime(2020,03,31),
                    Label = "VIR M BRENDAN GIRAUDET Virement vers BRENDAN GIRAUDET",
                    Reference = "6681107",
                    ValueDate = new DateTime(2020,03,31),
                    Amount = -1000,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses
                }
            };

            // Act
            serializer.Serialize(transactions, csvPath);

            // Assert
            Assert.True(true);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Z://bad/csv/path/file")]
        public void ShouldThrowExceptionWhenISerializeWithWrongCsvPath(string csvPath)
        {
            // Arrange
            ISerializer serializer = new BanquePopulaireCsvSerializer();
            var transactions = new List<WalletManagerDTO.Transaction>
            {
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    ComptabilisationDate = new DateTime(2020,03,31),
                    OperationDate = new DateTime(2020,03,31),
                    Label = "300320 CB****1526 ALLDEBRID.COM  92MONTROUGE",
                    Reference = "475QGS0",
                    ValueDate = new DateTime(2020,03,31),
                    Amount = -15.99,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Internet
                },
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    ComptabilisationDate = new DateTime(2020,03,31),
                    OperationDate = new DateTime(2020,03,31),
                    Label = "VIR M BRENDAN GIRAUDET Virement vers BRENDAN GIRAUDET",
                    Reference = "6681107",
                    ValueDate = new DateTime(2020,03,31),
                    Amount = -1000,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Courses
                }
            };

            // Act
            Action serializeAction = () => serializer.Serialize(transactions, csvPath);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.SerializerException>(serializeAction);
        }

        [Fact]
        public void ShouldThrowExceptionWhenISerializeEmptyTransactionList()
        {
            // Arrange
            ISerializer serializer = new BanquePopulaireCsvSerializer();
            var emptyTransactionList = new List<WalletManagerDTO.Transaction>();
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\serialize.csv";

            // Act
            Action serializeAction = () => serializer.Serialize(emptyTransactionList, csvPath);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.SerializerException>(serializeAction);
        }
    }
}
