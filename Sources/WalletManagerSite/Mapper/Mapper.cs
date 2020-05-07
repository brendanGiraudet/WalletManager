using System;
using System.Collections.Generic;
using System.Linq;
using WalletManagerSite.Models;

namespace WalletManagerServices.Mapper
{
    public class Mapper : IMapper
    {
        public TransactionChartViewModel MapToTransactionChartViewModel(WalletManagerDTO.Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException();
            }

            return new TransactionChartViewModel
            {
                Amount = transaction.Amount,
                Category = transaction.Category.ToString(),
            };
        }

        public IEnumerable<TransactionChartViewModel> MapToTransactionsChartViewModel(IEnumerable<WalletManagerDTO.Transaction> transactions)
        {
            return transactions.Select(MapToTransactionChartViewModel);
        }

        public IEnumerable<TransactionViewModel> MapToTransactionsViewModel(IEnumerable<WalletManagerDTO.Transaction> transactions)
        {
            return transactions.Select(MapToTransactionViewModel);
        }

        public TransactionViewModel MapToTransactionViewModel(WalletManagerDTO.Transaction transaction)
        {
            if(transaction == null)
            {
                throw new ArgumentNullException();
            }

            return new TransactionViewModel
            {
                Amount = transaction.Amount,
                Category = transaction.Category,
                Compte = transaction.Compte,
                Label = transaction.Label,
                OperationDate = transaction.OperationDate,
                Reference = transaction.Reference
            };
        }
    }
}
