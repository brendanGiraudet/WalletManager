using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WalletManagerDTO;
using WalletManagerSite.Models;

namespace WalletManagerSite.Tools.Mapper
{
    public class Mapper : IMapper
    {
        public CsvFileViewModel MapToCsvFileViewModel(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException();
            }

            return new CsvFileViewModel
            {
                CreatedDate = fileInfo.CreationTime,
                FileName = fileInfo.Name,
                FullPath = fileInfo.FullName,
                UpdateDate = fileInfo.LastWriteTime
            };
        }

        public TransactionChartViewModel MapToTransactionChartViewModel(WalletManagerDTO.Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException();
            }

            return new TransactionChartViewModel
            {
                Amount = transaction.Amount < 0 ? transaction.Amount * -1 : transaction.Amount,
                Category = transaction.Category.Name,
            };
        }

        public WalletManagerDTO.Transaction MapToTransactionDto(TransactionViewModel transactionViewModel)
        {
            if (transactionViewModel == null)
            {
                throw new ArgumentNullException();
            }

            return new WalletManagerDTO.Transaction
            {
                Amount = transactionViewModel.Amount,
                Category = transactionViewModel.Category,
                Compte = transactionViewModel.Compte,
                Label = transactionViewModel.Label,
                OperationDate = transactionViewModel.OperationDate,
                Reference = transactionViewModel.Reference
            };
        }

        public IEnumerable<TransactionChartViewModel> MapToTransactionsChartViewModel(IEnumerable<WalletManagerDTO.Transaction> transactions)
        {
            return transactions.Select(MapToTransactionChartViewModel);
        }

        public IEnumerable<TransactionViewModel> MapToTransactionViewModels(IEnumerable<WalletManagerDTO.Transaction> transactions)
        {
            return transactions.Select(MapToTransactionViewModel);
        }

        public TransactionsViewModel MapToTransactionsViewModel(List<TransactionViewModel> transactionsViewModel)
        {
            if (transactionsViewModel == null)
            {
                throw new ArgumentNullException();
            }

            return new TransactionsViewModel
            {
                Transactions = transactionsViewModel
            };
        }

        public TransactionViewModel MapToTransactionViewModel(WalletManagerDTO.Transaction transaction)
        {
            if (transaction == null)
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

        public SelectListItem MapToSelectListItem(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            return new SelectListItem
            {
                Value = category.Name,
                Text = category.Name
            };
        }

        public CategoryViewModel MapToCategoryViewModel(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            return new CategoryViewModel
            {
                CategoryName = category.Name,
                CreationDate = category.CreationDate
            };
        }
    }
}
