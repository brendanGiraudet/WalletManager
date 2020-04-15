using System.Collections.Generic;

namespace WalletManagerServices.Transaction
{
    public interface ITransactionServices
    {
        void LoadTransactions(string csvPath);

        List<WalletManagerDTO.Transaction> GetTransactions();

        WalletManagerDTO.Transaction GetTransaction(string reference);

        void UpdateTransaction(WalletManagerDTO.Transaction updatedTransaction);

        List<WalletManagerDTO.Transaction> GetGroupedTransactions();
    }
}
