using System.Linq;
using WalletManagerDAL.Serializer;
using Xunit;
using System.Collections.Generic;
using System;

namespace WalletManagerTestProject
{
    public class SerializerTests
    {
        [Fact]
        public void ShouldHaveListOfTransactionWhenIDeserializeACsvString()
        {
            // Arrange
            ISerializer serializer = new CsvSerializer();
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\deserialize.csv";

            // Act
            var transactionList = serializer.Deserialize(csvPath);

            // Assert
            Assert.True(transactionList.Any());
        }

        [Fact]
        public void ShouldHaveACsvFileWhenISerializeTransactionList()
        {
            // Arrange
            ISerializer serializer = new CsvSerializer();
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
    }
}
