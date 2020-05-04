using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class BanquePostaleCsvSerializer : ISerializer
    {
        public List<Transaction> Deserialize(string csvPath)
        {
            IEnumerable<string> csvLines;
            try
            {
                csvLines = File.ReadAllLines(csvPath);
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
            var csvLines = contentFile.Trim().Split("\n");
            return Deserialize(csvLines);
        }

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
                        Amount = Convert.ToDouble(values[2].Replace('.', ',')),
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

        private string GetCompteNumber(IEnumerable<string> csvLines)
        {
            var firstLine = csvLines.First();
            var splittedLine = firstLine.Split(';');
            return splittedLine.Last();
        }

        private static WalletManagerDTO.Enumerations.TransactionCategory GetCategory(string[] values)
        {
            try
            {
                Enum.TryParse(values.GetValue(4).ToString(), out WalletManagerDTO.Enumerations.TransactionCategory category);

                return category;
            }
            catch (Exception)
            {
                return WalletManagerDTO.Enumerations.TransactionCategory.NA;
            }
        }

        public void Serialize(List<Transaction> transactions, string csvPath)
        {
            if (transactions == null || !transactions.Any()) throw new WalletManagerDTO.Exceptions.SerializerException("Impossible to serialize an empty transaction list");

            StringWriter stringWriter = new StringWriter();
            stringWriter.Write(@"Numéro Compte   ;5380245H033
Type         ;CCP
Compte tenu en  ;euros
Date            ;03/05/2020
Solde (EUROS)   ;154,22
Solde (FRANCS)  ;1011,62

Date;Libellé;Montant(EUROS);Montant(FRANCS)");
            stringWriter.Write(stringWriter.NewLine);
            var delimiter = ";";

            foreach (var transaction in transactions)
            {
                stringWriter.Write(transaction.OperationDate + delimiter);
                stringWriter.Write(transaction.Label + delimiter);
                stringWriter.Write(transaction.Amount + delimiter);
                stringWriter.Write(" " + delimiter);
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
