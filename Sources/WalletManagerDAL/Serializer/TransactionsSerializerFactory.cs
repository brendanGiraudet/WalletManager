using System.Collections.Generic;
using System.Linq;
using WalletManagerDAL.File;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class TransactionsSerializerFactory
    {
        public ISerializer<Transaction> GetSerializer(IEnumerable<string> csvLines)
        {
            if (csvLines.Any(csvLine => csvLine.Contains("Montant(EUROS);Montant(FRANCS)")))
            {
                return new BanquePostaleTransactionsSerializer();
            }
            else if (csvLines.Any(csvLine => csvLine.Contains("Date valeur;Montant")))
            {
                return new BanquePopulaireTransactionsSerializer();
            }
            else if (csvLines.Any(csvLine => csvLine.Contains("Code de la banque")))
            {
                return new CaisseEpargneTransactionsSerializer();
            }
            else if(csvLines.Any(csvLine => csvLine.Contains("Compte;Date opération;")))
            {
                return new TransactionsSerializer(new FileService());
            }

            throw new WalletManagerDTO.Exceptions.SerializerException($"Unknow csv format");
        }

        public ISerializer<Transaction> GetSerializer()
        {
            return new TransactionsSerializer(new FileService());
        }
    }
}
