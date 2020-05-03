using System.Collections.Generic;
using System.Linq;

namespace WalletManagerSite.Models
{
    public class TransactionsViewModel
    {
        public List<TransactionViewModel> Transactions { get; set; }

        public string Date {
            get
            {
                return Transactions?.FirstOrDefault().OperationDate.ToString("Y");
            }
         }
    }
}
