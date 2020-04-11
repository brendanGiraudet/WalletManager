using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class TransactionSerializer : ISerializer
    {
        public List<Transaction> Deserialize(string csvPath)
        {
            var transactions = new List<Transaction>();
            try
            {
                var csvLines = File.ReadAllLines(csvPath).Skip(1);

                foreach (var csvLine in csvLines)
                {
                    string[] values = csvLine.Split(';');
                    transactions.Add(new Transaction
                    {
                        Compte = values[0],
                        ComptabilisationDate = Convert.ToDateTime(values[1]),
                        OperationDate = Convert.ToDateTime(values[2]),
                        Label = values[3],
                        Reference = values[4],
                        ValueDate = Convert.ToDateTime(values[5]),
                        Amount = Convert.ToDouble(values[6].Replace(',','.'))
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : impossible to deserialize the csv file in path { csvPath } due to " + ex.Message);
            }

            return transactions;
        }

        public void Serialize(List<Transaction> transactions, string path)
        {
            StringWriter stringWriter = new StringWriter();
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
                File.WriteAllText(path, stringWriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : impossible to serialize the csv file in path { path } due to " + ex.Message);
            }
        }
    }
}
