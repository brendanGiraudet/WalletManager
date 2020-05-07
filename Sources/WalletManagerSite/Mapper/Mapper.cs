using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalletManagerSite.Models;

namespace WalletManagerServices.Mapper
{
    public class Mapper : IMapper
    {
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
