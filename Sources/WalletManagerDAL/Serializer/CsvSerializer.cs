using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class CsvSerializer : ISerializer
    {
        public List<Transaction> Deserialize(string csvPath)
        {
            var transactions = new List<Transaction>();
            try
            {
                var csvLines = File.ReadAllLines(csvPath).Skip(1);

                foreach (var csvLine in csvLines)
                {
                    var values = csvLine.Split(';').ToArray();
                    var transaction = new Transaction
                    {
                        Compte = values[0],
                        ComptabilisationDate = Convert.ToDateTime(values[1]),
                        OperationDate = Convert.ToDateTime(values[2]),
                        Label = values[3],
                        Reference = values[4],
                        ValueDate = Convert.ToDateTime(values[5]),
                        Amount = Convert.ToDouble(values[6].Replace(',', '.'))
                    };
                    try
                    {
                        Enum.TryParse(values.GetValue(7).ToString(), out WalletManagerDTO.Enumerations.TransactionCategory category);

                        transaction.Category = category;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        transaction.Category = WalletManagerDTO.Enumerations.TransactionCategory.NA;
                    }
                    catch (ArgumentException)
                    {
                        transaction.Category = WalletManagerDTO.Enumerations.TransactionCategory.NA;
                    }
                    transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : impossible to deserialize the csv file in path { csvPath } due to " + ex.Message);
            }

            return transactions;
        }

        public void Serialize(List<Transaction> transactions, string csvPath)
        {
            StringWriter stringWriter = new StringWriter();
            stringWriter.Write("Compte;Date de comptabilisation;Date opération;Libellé;Référence;Date valeur;Montant;Category");
            var delimiter = ";";

            foreach (var transaction in transactions)
            {
                stringWriter.Write(transaction.Compte + delimiter);
                stringWriter.Write(transaction.ComptabilisationDate + delimiter);
                stringWriter.Write(transaction.OperationDate + delimiter);
                stringWriter.Write(transaction.Label + delimiter);
                stringWriter.Write(transaction.Reference + delimiter);
                stringWriter.Write(transaction.ValueDate + delimiter);
                stringWriter.Write(transaction.Amount + delimiter);
                stringWriter.Write(transaction.Category + delimiter);
                stringWriter.Write(stringWriter.NewLine);
            }

            try
            {
                File.WriteAllText(csvPath, stringWriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : impossible to serialize the csv file in path { csvPath } due to " + ex.Message);
            }
        }
    }
}
