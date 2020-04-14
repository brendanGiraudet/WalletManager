using System;
using System.Collections.Generic;
using System.Linq;

namespace WalletManagerServices.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        readonly WalletManagerDAL.Serializer.ISerializer _transactionSerializer;
        private List<WalletManagerDTO.Transaction> _transactions;

        public TransactionServices(WalletManagerDAL.Serializer.ISerializer transactionSerializer)
        {
            _transactionSerializer = transactionSerializer;
            _transactions = new List<WalletManagerDTO.Transaction>();
        }

        public void LoadTransactions(string csvPath)
        {
            _transactions = _transactionSerializer.Deserialize(csvPath);
        }

        public WalletManagerDTO.Transaction GetTransaction(string reference)
        {
            return _transactions.Find(transaction => transaction.Reference.Equals(reference));
        }

        public List<WalletManagerDTO.Transaction> GetTransactions()
        {
            var groupedTransactions = new List<WalletManagerDTO.Transaction>();
            foreach (var transactionToMerge in _transactions)
            {
                var transactionToMergeLabelLenght = transactionToMerge.Label.Length;
                var maxLetterTochangePourcent = 0.45;
                var numberOfMaxLetterToChange = transactionToMergeLabelLenght * maxLetterTochangePourcent;
                var findedTransaction = groupedTransactions.Find(t => LevenshteinDistanceCompute(t.Label,transactionToMerge.Label) <= numberOfMaxLetterToChange);
                if (findedTransaction == null)
                {
                    groupedTransactions.Add(transactionToMerge);
                }
                else
                {
                    findedTransaction.Amount += transactionToMerge.Amount;
                }
            }
            return groupedTransactions;
        }

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        private int LevenshteinDistanceCompute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public void UpdateTransaction(WalletManagerDTO.Transaction updatedTransaction)
        {
            WalletManagerDTO.Transaction findedTransaction;
            try
            {
                findedTransaction = _transactions.Find(t => t.Reference.Equals(updatedTransaction.Reference));
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.TransactionServiceException($"Impossible to update this transaction : {updatedTransaction} due to {ex.Message}");
            }

            if (findedTransaction == null) throw new WalletManagerDTO.Exceptions.TransactionServiceException($"Impossible to update transaction with this reference : {updatedTransaction.Reference}");

            findedTransaction.Category = updatedTransaction.Category;
        }
    }
}
