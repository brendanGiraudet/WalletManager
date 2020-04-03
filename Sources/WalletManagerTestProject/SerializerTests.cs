using System.Linq;
using WalletManagerServices.Serializer;
using Xunit;


namespace WalletManagerTestProject
{
    public class SerializerTests
    {
        [Fact]
        public void ShouldHaveListOfTransactionWhenISerializeACsvString()
        {
            // Arrange
            ISerializer serializer = new TransactionSerializer();
            var csvString = "31419185918;31/03/2020;31/03/2020;300320 CB****1526 ALLDEBRID.COM  92MONTROUGE;475QGS0;31/03/2020;-15\n31419185918; 31 / 03 / 2020; 31 / 03 / 2020; VIR M BRENDAN GIRAUDET Virement vers BRENDAN GIRAUDET; 6681107; 31 / 03 / 2020; -1000";

            // Act
            var transactionList = serializer.Serialize(csvString);

            // Assert
            Assert.True(transactionList.Any());
        }
    }
}
