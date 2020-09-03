﻿using System.Linq;
using WalletManagerDAL.Serializer;
using Xunit;
using System.Collections.Generic;
using System;
using System.IO;
using Bogus;
using WalletManagerDTO.Exceptions;

namespace WalletManagerTestProject
{
    public class CsvSerializerTests
    {
        const string csvBasePath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\Csv\";

        #region Transactions
        [Theory]
        [InlineData(csvBasePath + "banquePopulaire.csv")]
        [InlineData(csvBasePath + "banquePostale.csv")]
        [InlineData(csvBasePath + "deserialize.csv")]
        public void ShouldHaveListOfTransactionWhenIDeserializeACsvString(string csvPath)
        {
            // Arrange
            var serializer = new CsvSerializer();

            // Act
            var transactionList = serializer.Deserialize(csvPath);

            // Assert
            Assert.True(transactionList.Any());
        }

        [Theory]
        [InlineData(csvBasePath + "banquePopulaire.csv")]
        [InlineData(csvBasePath + "banquePostale.csv")]
        [InlineData(csvBasePath + "deserialize.csv")]
        public void ShouldHaveListOfTransactionWhenIDeserializeAstream(string csvPath)
        {
            // Arrange
            var serializer = new CsvSerializer();
            var expectedStream = File.OpenRead(csvPath);

            // Act
            var transactionList = serializer.Deserialize(expectedStream);

            // Assert
            Assert.True(transactionList.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(csvBasePath + "wrong.csv")]
        public void ShouldThrowExceptionWhenIDeserializeAWrongCsvFile(string csvPath)
        {
            // Arrange
            var serializer = new CsvSerializer();

            // Act
            Action deserializeAction = () => serializer.Deserialize(csvPath);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.SerializerException>(deserializeAction);
        }

        [Fact]
        public void ShouldSerializeTransactionList()
        {
            // Arrange
            var serializer = new CsvSerializer();
            var csvPath = csvBasePath + "serialize.csv";
            var transactions = new List<WalletManagerDTO.Transaction>
            {
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    OperationDate = new DateTime(2020,03,31),
                    Label = "300320 CB****1526 ALLDEBRID.COM  92MONTROUGE",
                    Reference = "475QGS0",
                    Amount = -15.99m,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Internet
                },
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    OperationDate = new DateTime(2020,03,31),
                    Label = "VIR M BRENDAN GIRAUDET Virement vers BRENDAN GIRAUDET",
                    Reference = "6681107",
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
            ISerializer serializer = new CsvSerializer();
            var transactions = new List<WalletManagerDTO.Transaction>
            {
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    OperationDate = new DateTime(2020,03,31),
                    Label = "300320 CB****1526 ALLDEBRID.COM  92MONTROUGE",
                    Reference = "475QGS0",
                    Amount = -15.99m,
                    Category = WalletManagerDTO.Enumerations.TransactionCategory.Internet
                },
                new WalletManagerDTO.Transaction
                {
                    Compte = "31419185918",
                    OperationDate = new DateTime(2020,03,31),
                    Label = "VIR M BRENDAN GIRAUDET Virement vers BRENDAN GIRAUDET",
                    Reference = "6681107",
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
            ISerializer serializer = new CsvSerializer();
            var emptyTransactionList = new List<WalletManagerDTO.Transaction>();
            var csvPath = csvBasePath + "serialize.csv";

            // Act
            Action serializeAction = () => serializer.Serialize(emptyTransactionList, csvPath);

            // Assert
            Assert.Throws<WalletManagerDTO.Exceptions.SerializerException>(serializeAction);
        }
        #endregion

        #region Categories
        [Fact]
        public void ShouldSerializeListOfCategory()
        {
            // Arrange
            var serializer = new CsvSerializer();
            var csvFilePath = Path.Combine(csvBasePath, "serializeCategory.csv");
            var faker = new Faker();
            var fakeCategories = new List<string>
            {
                faker.Random.String2(2),
                faker.Random.String2(2)
            };

            // Act
            serializer.Serialize(fakeCategories, csvFilePath);

            // Assert
            Assert.True(true);// no exception thrown
        }

        [Fact]
        public void ShouldThrowSerializerExceptionWhenSerializeWithEmptyCategoryList()
        {
            // Arrange
            var serializer = new CsvSerializer();
            var csvFilePath = Path.Combine(csvBasePath, "serializeCategory.csv");
            var emptyCategoryList = new List<string>();

            // Act
            Action serializeAction = () => serializer.Serialize(emptyCategoryList, csvFilePath);

            // Assert
            Assert.Throws<SerializerException>(serializeAction);
        }

        [Theory]
        [InlineData("")]
        [InlineData(csvBasePath + "wrongExtension.txt")]
        [InlineData("unknowFilePath.csv")]
        public void ShouldThrowSerializerExceptionWhenSerializeWithWrongFilePath(string csvFilePath)
        {
            // Arrange
            var serializer = new CsvSerializer();
            var faker = new Faker();
            var fakeCategories = new List<string>
            {
                faker.Random.String2(2),
                faker.Random.String2(2)
            };

            // Act
            Action serializeAction = () => serializer.Serialize(fakeCategories, csvFilePath);

            // Assert
            Assert.Throws<SerializerException>(serializeAction);
        }
        #endregion
    }
}
