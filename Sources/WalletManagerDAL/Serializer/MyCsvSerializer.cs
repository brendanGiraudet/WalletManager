using System;
using System.Collections.Generic;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class MyCsvSerializer : ICsvSerializer
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
                        OperationDate = Convert.ToDateTime(values[1]),
                        Label = values[2],
                        Reference = values[3],
                        Amount = Convert.ToDecimal(values[4].Replace('.', ',')),
                        Category = GetCategory(values)
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

        private static WalletManagerDTO.Enumerations.TransactionCategory GetCategory(string[] values)
        {
            try
            {
                Enum.TryParse(values.GetValue(5).ToString(), out WalletManagerDTO.Enumerations.TransactionCategory category);

                return category;
            }
            catch (Exception)
            {
                return WalletManagerDTO.Enumerations.TransactionCategory.NA;
            }
        }
    }
}
