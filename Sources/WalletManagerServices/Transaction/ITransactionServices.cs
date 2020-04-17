using System.Collections.Generic;

namespace WalletManagerServices.Transaction
{
    public interface ITransactionServices
    {
        void LoadTransactions(string csvPath);

        List<WalletManagerDTO.Transaction> GetTransactions();

        WalletManagerDTO.Transaction GetTransaction(string reference);

        void UpdateTransaction(WalletManagerDTO.Transaction updatedTransaction);

        List<WalletManagerDTO.Transaction> GetGroupedTransactionsByLabel();

        List<WalletManagerDTO.Transaction> GetDebitTransactions();

        List<WalletManagerDTO.Transaction> GetGroupedTransactionsByCategory(List<WalletManagerDTO.Transaction> transactions);

        void SaveTransactionsIntoCsvFile(string csvPath, List<WalletManagerDTO.Transaction> transactionsToSave);
    }
}
