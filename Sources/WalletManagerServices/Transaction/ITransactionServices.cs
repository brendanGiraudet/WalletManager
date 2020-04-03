using System;
using System.Collections.Generic;
using System.Text;

namespace WalletManagerServices.Transaction
{
    public interface ITransactionServices
    {
        IEnumerable<WalletManagerDTO.Transaction> GetTransactions();
    }
}
