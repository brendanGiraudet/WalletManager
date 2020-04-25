using System;
using System.Collections.Generic;

namespace WalletManagerSite.Models
{
    public class CompareViewModel
    {
        public IEnumerable<TransactionViewModel> ExpectedTransactions { get; set; }

        public IEnumerable<TransactionViewModel> ActualTransactions { get; set; }
    }
}
