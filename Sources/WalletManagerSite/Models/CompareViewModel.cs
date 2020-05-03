using System.Collections.Generic;

namespace WalletManagerSite.Models
{
    public class CompareViewModel
    {
        public List<TransactionsViewModel> TransactionsToCompare { get; set; } = new List<TransactionsViewModel>();

        public int ColumnSize { get
            {
                return 12 / TransactionsToCompare.Count;
            }
        }
    }
}
