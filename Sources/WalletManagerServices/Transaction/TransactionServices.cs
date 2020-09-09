using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDAL.Serializer;

namespace WalletManagerServices.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        ISerializer<WalletManagerDTO.Transaction> _transactionSerializer;
        private IEnumerable<WalletManagerDTO.Transaction> _transactions;

        public TransactionServices()
        {
            _transactions = new List<WalletManagerDTO.Transaction>();
        }

        public void LoadTransactions(string csvPath)
        {
            IEnumerable<string> csvLines = File.ReadAllLines(csvPath);
            _transactionSerializer = GetRightSerializer(csvLines);
            _transactions = _transactionSerializer.Deserialize(csvLines);
        }

        public void LoadTransactions(Stream stream)
        {
            var streamReader = new StreamReader(stream);
            var contentFile = streamReader.ReadToEnd();
            var csvLines = contentFile.Trim().Split("\n");
            _transactionSerializer = GetRightSerializer(csvLines);
            _transactions = _transactionSerializer.Deserialize(csvLines);
        }

        public WalletManagerDTO.Transaction GetTransaction(string reference)
        {
            return _transactions.FirstOrDefault(transaction => transaction.Reference.Equals(reference));
        }

        public IEnumerable<WalletManagerDTO.Transaction> GetTransactions()
        {
            return _transactions;
        }

        public IEnumerable<WalletManagerDTO.Transaction> GetGroupedTransactionsByLabel()
        {
            var maxLetterTochangePourcent = 0.20;
            IEnumerable<WalletManagerDTO.Transaction> copiedTransactions = GetTransactionsCopy(_transactions);
            var groupedTransactions = new List<WalletManagerDTO.Transaction>();

            foreach (var transactionToMerge in copiedTransactions)
            {
                var transactionToMergeLabelLenght = transactionToMerge.Label.Length;
                var numberOfMaxLetterToChange = transactionToMergeLabelLenght * maxLetterTochangePourcent;

                var isAlreadyMerge = groupedTransactions.Any(t => LevenshteinDistanceCompute(t.Label, transactionToMerge.Label) <= numberOfMaxLetterToChange);

                if (isAlreadyMerge) continue;

                var similarTransactions = copiedTransactions.Where(t => LevenshteinDistanceCompute(t.Label, transactionToMerge.Label) <= numberOfMaxLetterToChange && t.Reference != transactionToMerge.Reference).ToList();

                similarTransactions.ForEach(t =>
                {
                    transactionToMerge.Amount += t.Amount;
                });

                groupedTransactions.Add(transactionToMerge);
            }

            return groupedTransactions;
        }

        private IEnumerable<WalletManagerDTO.Transaction> GetTransactionsCopy(IEnumerable<WalletManagerDTO.Transaction> transactions)
        {
            return transactions.Select(t => new WalletManagerDTO.Transaction
            {
                Amount = t.Amount,
                Category = t.Category,
                Compte = t.Compte,
                Label = t.Label,
                OperationDate = t.OperationDate,
                Reference = t.Reference,
            }).ToList();
        }

        private string RemoveParasiteString(string label)
        {
            if (label.Contains("CB****1526"))
                return label.Remove(0, 10);

            return label;
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
                findedTransaction = _transactions.FirstOrDefault(t => t.Reference.Equals(updatedTransaction.Reference));
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.TransactionServiceException($"Impossible to update this transaction : {updatedTransaction} due to {ex.Message}");
            }

            if (findedTransaction == null) throw new WalletManagerDTO.Exceptions.TransactionServiceException($"Impossible to update transaction with this reference : {updatedTransaction.Reference}");

            findedTransaction.Category = updatedTransaction.Category;
        }

        public IEnumerable<WalletManagerDTO.Transaction> GetDebitTransactions()
        {
            return _transactions.Where(t => t.Amount < 0).ToList();
        }

        public IEnumerable<WalletManagerDTO.Transaction> GetGroupedTransactionsByCategory(IEnumerable<WalletManagerDTO.Transaction> transactions)
        {
            var maxLetterTochangePourcent = 0.20;
            IEnumerable<WalletManagerDTO.Transaction> copiedTransactions = GetTransactionsCopy(transactions);
            var groupedTransactions = new List<WalletManagerDTO.Transaction>();

            foreach (var transactionToMerge in copiedTransactions)
            {
                var transactionToMergeLabelLenght = transactionToMerge.Label.Length;
                var numberOfMaxLetterToChange = transactionToMergeLabelLenght * maxLetterTochangePourcent;

                var isAlreadyMerge = groupedTransactions.Any(t => t.Category.Equals(transactionToMerge.Category));

                if (isAlreadyMerge) continue;

                var similarTransactions = copiedTransactions.Where(t => t.Category.Equals(transactionToMerge.Category) && t.Reference != transactionToMerge.Reference).ToList();

                similarTransactions.ForEach(t =>
                {
                    transactionToMerge.Amount += t.Amount;
                });

                groupedTransactions.Add(transactionToMerge);
            }

            return groupedTransactions;
        }

        public void SaveTransactionsIntoCsvFile(string csvPath, IEnumerable<WalletManagerDTO.Transaction> transactionsToSave)
        {
            try
            {
                _transactionSerializer.Serialize(transactionsToSave, csvPath);
            }
            catch (Exception ex)
            {
                throw new WalletManagerDTO.Exceptions.TransactionServiceException($"Impossible to save transactions due to {ex.Message}", ex);
            }
        }

        public void SaveTransactionsIntoCsvFile(string csvPath)
        {
            SaveTransactionsIntoCsvFile(csvPath, _transactions);
        }

        public void Delete(string reference)
        {
            _transactions = _transactions.Where(t => t.Reference != reference);
        }

        public IEnumerable<WalletManagerDTO.Transaction> GetTransactions(string csvPath)
        {
            var lines = File.ReadAllLines(csvPath);
            _transactionSerializer = GetRightSerializer(lines);
            return _transactionSerializer.Deserialize(lines);
        }

        public IEnumerable<WalletManagerDTO.Transaction> FusionTransactions(IEnumerable<WalletManagerDTO.Transaction> firstTransactionListToFusion, IEnumerable<WalletManagerDTO.Transaction> secondTransactionListToFusion)
        {
            return firstTransactionListToFusion.Concat(secondTransactionListToFusion).ToList();
        }

        public void SetTransactions(IEnumerable<WalletManagerDTO.Transaction> transactions)
        {
            _transactions = transactions;
        }

        private ISerializer<WalletManagerDTO.Transaction> GetRightSerializer(IEnumerable<string> csvLines)
        {
            if (csvLines.Any(csvLine => csvLine.Contains("Montant(EUROS);Montant(FRANCS)")))
            {
                return new BanquePostaleTransactionsSerializer();
            }
            else if (csvLines.Any(csvLine => csvLine.Contains("Date valeur;Montant")))
            {
                return new BanquePopulaireTransactionsSerializer();
            }
            else
            {
                return new TransactionsSerializer();
            }
        }
    }
}
