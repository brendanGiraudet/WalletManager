using System.Linq;
using WalletManagerDAL.Serializer;
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
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\serialize.csv";

            // Act
            var transactionList = serializer.Deserialize(csvPath);

            // Assert
            Assert.True(transactionList.Any());
        }
    }
}
