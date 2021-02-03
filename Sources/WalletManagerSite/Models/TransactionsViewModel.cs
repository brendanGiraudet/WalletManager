using System.Collections.Generic;
using System.Linq;

namespace WalletManagerSite.Models
{
    public class TransactionsViewModel
    {
        public IEnumerable<TransactionViewModel> Transactions { get; set; }

        public System.DateTime? Date {
            get
            {
                var transaction = Transactions.FirstOrDefault(t => t.Amount > 0);
                return transaction?.OperationDate;
            }
         }
    }
}
