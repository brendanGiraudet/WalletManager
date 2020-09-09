using System.Collections.Generic;
using System.IO;

namespace WalletManagerServices.Transaction
{
    public interface ITransactionServices
    {
        void LoadTransactions(string csvPath);

        void LoadTransactions(Stream stream);

        IEnumerable<WalletManagerDTO.Transaction> GetTransactions();

        WalletManagerDTO.Transaction GetTransaction(string reference);

        void UpdateTransaction(WalletManagerDTO.Transaction updatedTransaction);

        IEnumerable<WalletManagerDTO.Transaction> GetGroupedTransactionsByLabel();

        IEnumerable<WalletManagerDTO.Transaction> GetDebitTransactions();

        IEnumerable<WalletManagerDTO.Transaction> GetGroupedTransactionsByCategory(IEnumerable<WalletManagerDTO.Transaction> transactions);

        void SaveTransactionsIntoCsvFile(string csvPath, IEnumerable<WalletManagerDTO.Transaction> transactionsToSave);

        void SaveTransactionsIntoCsvFile(string csvPath);

        void Delete(string reference);
        IEnumerable<WalletManagerDTO.Transaction> GetTransactions(string csvPath);

        IEnumerable<WalletManagerDTO.Transaction> FusionTransactions(IEnumerable<WalletManagerDTO.Transaction> firstTransactionListToFusion, IEnumerable<WalletManagerDTO.Transaction> secondTransactionListToFusion);

        void SetTransactions(IEnumerable<WalletManagerDTO.Transaction> transactions);
    }
}
