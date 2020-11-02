using System.Collections.Generic;
using System.Linq;

namespace WalletManagerSite.Models
{
    public class CompareViewModel
    {
        public IEnumerable<TransactionsViewModel> TransactionsToCompare { get; set; } = Enumerable.Empty<TransactionsViewModel>();

        public int ColumnSize { get
            {
                return 12 / TransactionsToCompare.Count();
            }
        }
    }
}
