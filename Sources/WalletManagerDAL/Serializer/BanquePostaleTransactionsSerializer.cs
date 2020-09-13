using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class BanquePostaleTransactionsSerializer : ISerializer<Transaction>
    {
        IEnumerable<Transaction> ISerializer<Transaction>.Deserialize(IEnumerable<string> lines)
        {
            var transactions = new List<Transaction>();

            try
            {
                var compteNumber = GetCompteNumber(lines);
                lines = lines.Skip(8); // Skip header
                foreach (var line in lines)
                {
                    var values = line.Split(';').ToArray();
                    var transaction = new Transaction
                    {
                        Compte = compteNumber,
                        OperationDate = Convert.ToDateTime(values[0]),
                        Label = values[1],
                        Reference = Guid.NewGuid().ToString(),
                        Amount = Convert.ToDecimal(values[2].Replace(',', '.')),
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

        private string GetCompteNumber(IEnumerable<string> csvLines)
        {
            var firstLine = csvLines.First();
            var splittedLine = firstLine.Split(';');
            return splittedLine.Last().Trim('\r');
        }

        public bool Serialize(IEnumerable<Transaction> objects, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
