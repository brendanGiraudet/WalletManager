using System;
using System.Collections.Generic;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class CaisseEpargneTransactionsSerializer : ISerializer<Transaction>
    {
        public bool Serialize(IEnumerable<Transaction> objects, string filePath)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Transaction> ISerializer<Transaction>.Deserialize(IEnumerable<string> lines)
        {
            var transactions = new List<Transaction>();

            try
            {
                var compteLine = lines.ElementAt(1).Split(';');
                var comptePartLine = compteLine[0].Split(':');
                var compte = comptePartLine[1];

                lines = lines.Skip(5);// Skip header
                lines = lines.SkipLast(1);// Skip footer

                foreach (var line in lines)
                {
                    var values = line.Split(';').ToArray();
                    var debit = values[3];
                    var credit = values[4];
                    var amount = string.IsNullOrEmpty(debit) ? credit : debit;

                    var transaction = new Transaction
                    {
                        Compte = compte,
                        OperationDate = Convert.ToDateTime(values[0]),
                        Label = values[2],
                        Reference = Guid.NewGuid().ToString(),
                        Amount = Convert.ToDecimal(amount),
                        Category = new Category()
                    };

                    transactions.Add(transaction);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Wrong caisse epargne csv format", ex);
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Error : impossible to deserialize due to " + ex.Message, ex);
            }

            return transactions;
        }
    }
}
