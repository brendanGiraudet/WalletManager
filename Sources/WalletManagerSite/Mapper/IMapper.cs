
using System.Collections.Generic;

namespace WalletManagerServices.Mapper
{
    public interface IMapper
    {
        WalletManagerSite.Models.TransactionViewModel MapToTransactionViewModel(WalletManagerDTO.Transaction transaction);

        IEnumerable<WalletManagerSite.Models.TransactionViewModel> MapToTransactionsViewModel(IEnumerable<WalletManagerDTO.Transaction> transactions);
    }
}
