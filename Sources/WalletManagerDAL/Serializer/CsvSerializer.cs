using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO;

namespace WalletManagerDAL.Serializer
{
    public class CsvSerializer : ISerializer
    {
        readonly string _columnSeparator = ";";
        readonly string _fileExtension = ";";

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

            foreach (var transaction in transactions)
            {
                stringWriter.Write(transaction.Compte + _columnSeparator);
                stringWriter.Write(transaction.OperationDate + _columnSeparator);
                stringWriter.Write(transaction.Label + _columnSeparator);
                stringWriter.Write(transaction.Reference + _columnSeparator);
                stringWriter.Write(transaction.Amount + _columnSeparator);
                stringWriter.Write(transaction.Category + _columnSeparator);
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
        
        public void Serialize(IEnumerable<string> categories, string csvPath)
        {
            if (categories == null || !categories.Any()) throw new WalletManagerDTO.Exceptions.SerializerException("Impossible to serialize an empty category list");

            var fileExtension = Path.GetExtension(csvPath);
            if(!fileExtension.Equals(_fileExtension)) throw new WalletManagerDTO.Exceptions.SerializerException($"Impossible to serialize in file with extension  {fileExtension}, you can use {_fileExtension} extension file");

            var csvContent = string.Join(_columnSeparator, categories);

            try
            {
                File.WriteAllText(csvPath, csvContent);
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.SerializerException($"Error : impossible to serialize into csv file in path { csvPath } due to " + ex.Message, ex);
            }
        }
    }
}
