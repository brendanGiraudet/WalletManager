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
            IEnumerable<string> csvLines;
            try
            {
                csvLines = File.ReadAllLines(csvPath).Skip(1);
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Wrong csv path : {csvPath}", ex);
            }
            return Deserialize(csvLines);
        }

        public List<Transaction> Deserialize(Stream stream)
        {
            if (stream == null) return new List<Transaction>();

            var streamReader = new StreamReader(stream);
            var contentFile = streamReader.ReadToEnd();
            var csvLines = contentFile.Trim().Split("\n").Skip(1);
            return Deserialize(csvLines);
        }

        public List<Transaction> Deserialize(IEnumerable<string> csvLines)
        {
            var transactions = new List<Transaction>();

            try
            {
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
                        Amount = Convert.ToDouble(values[6].Replace('.', ',')),
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
                Enum.TryParse(values.GetValue(7).ToString(), out WalletManagerDTO.Enumerations.TransactionCategory category);

                return category;
            }
            catch (ArgumentException)
            {
                return WalletManagerDTO.Enumerations.TransactionCategory.NA;
            }
        }

        public void Serialize(List<Transaction> transactions, string csvPath)
        {
            if (transactions == null || !transactions.Any()) throw new WalletManagerDTO.Exceptions.SerializerException("Impossible to serialize an empty transaction list");

            StringWriter stringWriter = new StringWriter();
            stringWriter.Write("Compte;Date de comptabilisation;Date opération;Libellé;Référence;Date valeur;Montant;Category");
            stringWriter.Write(stringWriter.NewLine);
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
                throw new WalletManagerDTO.Exceptions.SerializerException($"Error : impossible to serialize into csv file in path { csvPath } due to " + ex.Message, ex);
            }
        }
    }
}
