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
                csvLines = File.ReadAllLines(csvPath);
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Wrong csv path : {csvPath}", ex);
            }

            ICsvSerializer serializer = GetRightSerializer(csvLines);

            return serializer.Deserialize(csvLines);
        }

        private ICsvSerializer GetRightSerializer(IEnumerable<string> csvLines)
        {
            if (csvLines.Any(csvLine => csvLine.Contains("Montant(EUROS);Montant(FRANCS)")))
            {
                return new BanquePostaleCsvSerializer();
            }
            else if(csvLines.Any(csvLine => csvLine.Contains("Date valeur;Montant")))
            {
                return new BanquePopulaireCsvSerializer();
            }
            else
            {
                return new MyCsvSerializer();
            }
        }

        public List<Transaction> Deserialize(Stream stream)
        {
            if (stream == null) return new List<Transaction>();

            var streamReader = new StreamReader(stream);
            var contentFile = streamReader.ReadToEnd();
            var csvLines = contentFile.Trim().Split("\n");

            var serializer = GetRightSerializer(csvLines);

            return serializer.Deserialize(csvLines);
        }

        public List<Transaction> Deserialize(IEnumerable<string> csvLines)
        {
            var serializer = GetRightSerializer(csvLines);

            return serializer.Deserialize(csvLines);
        }

        public void Serialize(List<Transaction> transactions, string csvPath)
        {
            if (transactions == null || !transactions.Any()) throw new WalletManagerDTO.Exceptions.SerializerException("Impossible to serialize an empty transaction list");

            StringWriter stringWriter = new StringWriter();
            stringWriter.Write("Compte;Date opération;Libellé;Référence;Montant;Categorie");
            stringWriter.Write(stringWriter.NewLine);
            var delimiter = ";";

            foreach (var transaction in transactions)
            {
                stringWriter.Write(transaction.Compte + delimiter);
                stringWriter.Write(transaction.OperationDate + delimiter);
                stringWriter.Write(transaction.Label + delimiter);
                stringWriter.Write(transaction.Reference + delimiter);
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
