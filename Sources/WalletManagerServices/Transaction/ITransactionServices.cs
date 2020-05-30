using System.Collections.Generic;
using System.IO;

namespace WalletManagerServices.Transaction
{
    public interface ITransactionServices
    {
        void LoadTransactions(string csvPath);

        void LoadTransactions(Stream stream);

        List<WalletManagerDTO.Transaction> GetTransactions();

        WalletManagerDTO.Transaction GetTransaction(string reference);

        void UpdateTransaction(WalletManagerDTO.Transaction updatedTransaction);

        List<WalletManagerDTO.Transaction> GetGroupedTransactionsByLabel();

        List<WalletManagerDTO.Transaction> GetDebitTransactions();

        List<WalletManagerDTO.Transaction> GetGroupedTransactionsByCategory(List<WalletManagerDTO.Transaction> transactions);

        void SaveTransactionsIntoCsvFile(string csvPath, List<WalletManagerDTO.Transaction> transactionsToSave);

        void SaveTransactionsIntoCsvFile(string csvPath);

        void Delete(string reference);
        List<WalletManagerDTO.Transaction> GetTransactions(string csvPath);
        
        List<WalletManagerDTO.Transaction> FusionTransactions(List<WalletManagerDTO.Transaction> firstTransactionListToFusion, List<WalletManagerDTO.Transaction> secondTransactionListToFusion);


    }
}
