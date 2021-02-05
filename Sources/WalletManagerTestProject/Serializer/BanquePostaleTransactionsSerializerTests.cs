using WalletManagerDAL.Serializer;
using Xunit;
using System.IO;
using System.Linq;
using WalletManagerDTO;
using System.Collections.Generic;

namespace WalletManagerTestProject.Serializer
{
    public class BanquePostaleTransactionsSerializerTests
    {
        private IEnumerable<string> _contentFile = Enumerable.Empty<string>();

        readonly ISerializer<Transaction> _serializer;

        public BanquePostaleTransactionsSerializerTests()
        {
            _serializer = new BanquePostaleTransactionsSerializer();
            _contentFile = _contentFile.Append("Numéro Compte   ;5380245H033");
            _contentFile = _contentFile.Append("Type         ; CCP");
            _contentFile = _contentFile.Append("Compte tenu en  ;euros");
            _contentFile = _contentFile.Append("Date            ;08/09/2020");
            _contentFile = _contentFile.Append("Solde (EUROS)   ;887,61");
            _contentFile = _contentFile.Append("Solde (FRANCS)  ;5822,34");
            _contentFile = _contentFile.Append("");
            _contentFile = _contentFile.Append("Date;Libellé;Montant(EUROS);Montant(FRANCS)");
            _contentFile = _contentFile.Append("07/09/2020;\"VIREMENT POUR WATTIER COMPTE FR76182060040819811126001 LOYER\";-680,00;-4460,51");
            _contentFile = _contentFile.Append("04/09/2020;\"VIREMENT DE MME   NATHALIA ROHEE Virement de MME NATHALIA ROHEE REFERENCE: 0190248000073017 \";850,00;5575,63");
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
