using System;
using System.Collections.Generic;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class BanquePopulaireCsvSerializer : ICsvSerializer
    {
        public List<Transaction> Deserialize(IEnumerable<string> csvLines)
        {
            var transactions = new List<Transaction>();

            try
            {
                csvLines = csvLines.Skip(1);
                foreach (var csvLine in csvLines)
                {
                    var values = csvLine.Split(';').ToArray();
                    var transaction = new Transaction
                    {
                        Compte = values[0],
                        OperationDate = Convert.ToDateTime(values[2]),
                        Label = values[3],
                        Reference = values[4],
                        Amount = Convert.ToDecimal(values[6]),
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
    }
}
