using System.Collections.Generic;
using System.IO;
using WalletManagerSite.Models;

namespace WalletManagerSite.Tools.Mapper
{
    public interface IMapper
    {
        TransactionViewModel MapToTransactionViewModel(WalletManagerDTO.Transaction transaction);

        IEnumerable<TransactionViewModel> MapToTransactionViewModels(IEnumerable<WalletManagerDTO.Transaction> transactions);
        
        TransactionChartViewModel MapToTransactionChartViewModel(WalletManagerDTO.Transaction transaction);

        IEnumerable<TransactionChartViewModel> MapToTransactionsChartViewModel(IEnumerable<WalletManagerDTO.Transaction> transactions);

        WalletManagerDTO.Transaction MapToTransactionDto(TransactionViewModel transactionViewModel);
        
        TransactionsViewModel MapToTransactionsViewModel(List<TransactionViewModel> transactionsViewModel);
        
        CsvFileViewModel MapToCsvFileViewModel(FileInfo fileInfo);
    }
}
