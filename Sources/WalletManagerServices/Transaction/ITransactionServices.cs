using System;
using System.Collections.Generic;
using System.Text;

namespace WalletManagerServices.Transaction
{
    public interface ITransactionServices
    {
        List<WalletManagerDTO.Transaction> GetTransactions();

        WalletManagerDTO.Transaction GetTransaction(string reference);
    }
}
