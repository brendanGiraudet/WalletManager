using System;
using System.Collections.Generic;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class BanquePostaleCsvSerializer : ICsvSerializer
    {
        public List<Transaction> Deserialize(IEnumerable<string> csvLines)
        {
            var transactions = new List<Transaction>();

            try
            {
                var compteNumber = GetCompteNumber(csvLines);
                csvLines = csvLines.Skip(8);
                foreach (var csvLine in csvLines)
                {
                    var values = csvLine.Split(';').ToArray();
                    var transaction = new Transaction
                    {
                        Compte = compteNumber,
                        OperationDate = Convert.ToDateTime(values[0]),
                        Label = values[1],
                        Reference = Guid.NewGuid().ToString(),
                        Amount = Convert.ToDecimal(values[2].Replace(',', '.')),
                        Category = WalletManagerDTO.Enumerations.TransactionCategory.NA
                    };

                    transactions.Add(transaction);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Wrong csv format", ex);
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Error : impossible to deserialize the csv file due to " + ex.Message, ex);
            }

            return transactions;
        }

        private string GetCompteNumber(IEnumerable<string> csvLines)
        {
            var firstLine = csvLines.First();
            var splittedLine = firstLine.Split(';');
            return splittedLine.Last();
        }
    }
}
