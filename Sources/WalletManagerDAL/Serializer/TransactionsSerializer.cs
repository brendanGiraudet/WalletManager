using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO;
using WalletManagerDTO.Exceptions;

namespace WalletManagerDAL.Serializer
{
    public class TransactionsSerializer : ISerializer<Transaction>
    {
        readonly char _columnSeparator = ';';

        bool ISerializer<Transaction>.Serialize(IEnumerable<Transaction> transactions, string filePath)
        {
            if (transactions == null || !transactions.Any()) throw new SerializerException("Impossible to serialize an empty transaction list");

            StringWriter stringWriter = new StringWriter();
            stringWriter.Write("Compte;Date opération;Libellé;Référence;Montant;Categorie");
            stringWriter.Write(stringWriter.NewLine);

            foreach (var transaction in transactions)
            {
                stringWriter.Write(transaction.Compte + _columnSeparator);
                stringWriter.Write(transaction.OperationDate.ToString() + _columnSeparator);
                stringWriter.Write(transaction.Label + _columnSeparator);
                stringWriter.Write(transaction.Reference + _columnSeparator);
                stringWriter.Write(transaction.Amount.ToString() + _columnSeparator);
                stringWriter.Write(transaction.Category + _columnSeparator);
                stringWriter.Write(stringWriter.NewLine);
            }

            try
            {
                File.WriteAllText(filePath, stringWriter.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new SerializerException($"Error : impossible to serialize into transactions in path { filePath } due to " + ex.Message, ex);
            }
        }

        IEnumerable<Transaction> ISerializer<Transaction>.Deserialize(IEnumerable<string> lines)
        {
            var transactions = new List<Transaction>();

            try
            {
                lines = lines.Skip(1);// Skip header
                foreach (var line in lines)
                {
                    var values = line.Split(';').ToArray();
                    var compte = values[0];
                    var operationDate = Convert.ToDateTime(values[1]);
                    var label = values[2];
                    var reference = values[3];
                    var amount = Convert.ToDecimal(values[4]);
                    var category = values[5];
                    var transaction = new Transaction
                    {
                        Compte = compte,
                        OperationDate = operationDate,
                        Label = label,
                        Reference = reference,
                        Amount = amount,
                        Category = category
                    };

                    transactions.Add(transaction);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new SerializerException($"Wrong csv format", ex);
            }
            catch (Exception ex)
            {
                throw new SerializerException($"Error : impossible to deserialize the csv file due to " + ex.Message, ex);
            }

            return transactions;
        }
    }
}
