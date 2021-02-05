using WalletManagerDAL.Serializer;
using Xunit;
using System.IO;
using System.Linq;
using WalletManagerDTO;
using System.Collections.Generic;

namespace WalletManagerTestProject.Serializer
{
    public class CaisseEpargneTransactionsSerializerTests
    {
        private IEnumerable<string> _contentFile = Enumerable.Empty<string>();

        readonly ISerializer<Transaction> _serializer;

        public CaisseEpargneTransactionsSerializerTests()
        {
            _serializer = new CaisseEpargneTransactionsSerializer();
            _contentFile = _contentFile.Append("Code de la banque : 14505;Code de l'agence : 00001;Date de début de téléchargement : 01/10/2020;Date de fin de téléchargement : 31/10/2020;");
            _contentFile = _contentFile.Append("Numéro de compte : 04309621484;Nom du compte : COMPTE CHEQUE;Devise : EUR;");
            _contentFile = _contentFile.Append("");
            _contentFile = _contentFile.Append("Solde en fin de période;;;;3480,82");
            _contentFile = _contentFile.Append("Date;Numéro d'opération;Libellé;Débit;Crédit;Détail;");
            _contentFile = _contentFile.Append("16-Jun-20 12:36:14 AM;2810202020201028-17.58.04.478319 -;CB L ORIENT PALACE FACT 271020;-12,10;;CB L ORIENT PALACE FACT 271020 ;");
            _contentFile = _contentFile.Append("16-Jun-20 12:36:14 AM;2710202020201027-09.25.47.953651 -;CB INTERMARCHE     FACT 261020;-96,31;;CB INTERMARCHE     FACT 261020 ;");
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
