using Configuration;
using System;
using System.Collections.Generic;

namespace WalletManagerServices.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        readonly Serializer.ISerializer _transactionSerializer;
        readonly IConfigurator _configurator;

        public TransactionServices(Serializer.ISerializer transactionSerializer, IConfigurator configurator)
        {
            _transactionSerializer = transactionSerializer;
            _configurator = configurator;
        }

        public WalletManagerDTO.Transaction GetTransaction(string reference)
        {
            var csvPath = _configurator.GetCsvPath();
            var transactions = _transactionSerializer.Deserialize(csvPath);

            return transactions.Find(transaction => transaction.Reference.Equals(reference));
        }

        public List<WalletManagerDTO.Transaction> GetTransactions()
        {
            var csvPath = _configurator.GetCsvPath();
            return _transactionSerializer.Deserialize(csvPath);
        }
    }
}
