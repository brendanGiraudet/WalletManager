﻿using System;
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

        public void Serialize(List<Transaction> transactions)
        {
            throw new NotImplementedException();
        }
    }
}