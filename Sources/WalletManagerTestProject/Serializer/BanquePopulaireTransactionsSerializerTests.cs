using System.Collections.Generic;
using System.Linq;
using WalletManagerDAL.Serializer;
using WalletManagerDTO;
using Xunit;

namespace WalletManagerTestProject.Serializer
{
    public class BanquePopulaireTransactionsSerializerTests
    {
        private IEnumerable<string> _contentFile = Enumerable.Empty<string>();

        readonly ISerializer<Transaction> _serializer;

        public BanquePopulaireTransactionsSerializerTests()
        {
            _serializer = new BanquePopulaireTransactionsSerializer();
            _contentFile = _contentFile.Append("Compte;Date de comptabilisation;Date opération;Libellé;Référence;Date valeur;Montant");
            _contentFile = _contentFile.Append("31419185918;16-Jun-20 12:36:14 AM;16-Jun-20 12:36:14 AM;VIR M BRENDAN GIRAUDET Virement vers BRENDAN GIRAUDET;9896465;31/08/2020;-600,00");
            _contentFile = _contentFile.Append("31419185918;16-Jun-20 12:36:14 AM;16-Jun-20 12:36:14 AM;290820 CB****1526 HALLES COIGNIER78COIGNIERES; B870NZD;31/08/2020;-47,07;");
        }

        #region Deserialize
        [Fact]
        public void ShouldDeserializeListOfTransaction()
        {
            // Arrange

            // Act
            var transactions = _serializer.Deserialize(_contentFile);

            // Assert
            Assert.True(transactions.Any());
        }
        #endregion
    }
}
