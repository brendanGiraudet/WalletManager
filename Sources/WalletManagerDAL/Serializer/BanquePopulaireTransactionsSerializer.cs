using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class BanquePopulaireTransactionsSerializer : ISerializer<Transaction>
    {
        public Task<bool> Serialize(IEnumerable<Transaction> objects, string filePath)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Transaction> ISerializer<Transaction>.Deserialize(IEnumerable<string> lines)
        {
            var transactions = new List<Transaction>();

            try
            {
                lines = lines.Skip(1);// Skip header
                foreach (var line in lines)
                {
                    var values = line.Split(';').ToArray();
                    var transaction = new Transaction
                    {
                        Compte = values[0],
                        OperationDate = Convert.ToDateTime(values[2]),
                        Label = values[3],
                        Reference = Guid.NewGuid().ToString(),
                        Amount = Convert.ToDecimal(values[6]),
                        Category = new Category()
                    };

                    transactions.Add(transaction);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Wrong banque populaire csv format", ex);
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Error : impossible to deserialize due to " + ex.Message, ex);
            }

            return transactions;
        }
    }
}
