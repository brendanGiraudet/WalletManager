using System.Collections.Generic;
using System.Linq;

namespace WalletManagerSite.Models
{
    public class TransactionsViewModel
    {
        public List<TransactionViewModel> Transactions { get; set; }

        public System.DateTime? Date {
            get
            {
                return Transactions?.FirstOrDefault().OperationDate;
            }
         }
    }
}
