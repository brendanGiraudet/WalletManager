
using System.Collections.Generic;
using WalletManagerSite.Models;

namespace WalletManagerServices.Mapper
{
    public interface IMapper
    {
        TransactionViewModel MapToTransactionViewModel(WalletManagerDTO.Transaction transaction);

        IEnumerable<TransactionViewModel> MapToTransactionsViewModel(IEnumerable<WalletManagerDTO.Transaction> transactions);
        
        TransactionChartViewModel MapToTransactionChartViewModel(WalletManagerDTO.Transaction transaction);

        IEnumerable<TransactionChartViewModel> MapToTransactionsChartViewModel(IEnumerable<WalletManagerDTO.Transaction> transactions);

        WalletManagerDTO.Transaction MapToTransactionDto(TransactionViewModel transactionViewModel);
        
        TransactionsViewModel MapToTransactionsViewModel(List<TransactionViewModel> transactionsViewModel);
    }
}
