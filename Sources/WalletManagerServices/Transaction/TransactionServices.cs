using System.Collections.Generic;

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
            return _transactions;
        }
    }
}
