using System;
using System.Collections.Generic;

namespace WalletManagerServices.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        readonly Serializer.ISerializer _transactionSerializer;

        public TransactionServices(Serializer.ISerializer transactionSerializer)
        {
            _transactionSerializer = transactionSerializer;
        }

        public List<WalletManagerDTO.Transaction> GetTransactions()
        {
            var csvPath = @"D:\document\project\WalletManager\Sources\WalletManagerTestProject\CSV\serialize.csv";
            return _transactionSerializer.Deserialize(csvPath);
        }
    }
}
