using System.Linq;
using WalletManagerServices.Serializer;
using Xunit;


namespace WalletManagerTestProject
{
    public class SerializerTests
    {
        [Fact]
        public void ShouldHaveListOfTransactionWhenIDeserializeACsvString()
        {
            // Arrange
            ISerializer serializer = new TransactionSerializer();
            var csvString = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\serialize.csv";

            // Act
            var transactionList = serializer.Deserialize(csvString);

            // Assert
            Assert.True(transactionList.Any());
        }
    }
}
