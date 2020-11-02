using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using WalletManagerDTO;
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
        
        TransactionsViewModel MapToTransactionsViewModel(IEnumerable<TransactionViewModel> transactionsViewModel);
        
        CsvFileViewModel MapToCsvFileViewModel(FileInfo fileInfo);

        SelectListItem MapToSelectListItem(Category category);

        CategoryViewModel MapToCategoryViewModel(Category category);
        
        Category MapToCategory(CategoryViewModel categoryViewModel);
    }
}
