using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using WalletManagerDTO;
using WalletManagerDTO.Enumerations;
using WalletManagerServices.Transaction;
using WalletManagerSite.Models;

namespace WalletManagerSite.Controllers
{
    public class CompareController : Controller
    {
        readonly IConfiguration _configuration;
        readonly ITransactionServices _transactionServices;
        private readonly IStringLocalizer<CompareController> _localizer;

        public CompareController(IConfiguration configuration, ITransactionServices transactionServices, IStringLocalizer<CompareController> localizer)
        {
            _configuration = configuration;
            _transactionServices = transactionServices;
            _localizer = localizer;
        }
        // GET: Compare
        public ActionResult Index()
        {
            return View(GetCsvList());
        }

        public ActionResult Compare(List<CsvFileViewModel> csvFiles)
        {
            var compareTransactionList = new CompareViewModel();

            var selectedCsvFiles = csvFiles.Where(c => c.IsChecked).ToList();
            if (selectedCsvFiles == null)
            {
                ViewBag.Error = _localizer["EmptySelectedCsvFileError"];
                return View(compareTransactionList);
            }

            if (selectedCsvFiles.Count < 2 || selectedCsvFiles.Count > 4)
            {
                ViewBag.Error = _localizer["BadNumberOfCsv"];
                return View(compareTransactionList);
            }

            foreach (var selectedFile in selectedCsvFiles)
            {
                var filePath = GetFullFilePath(selectedFile.FileName);
                try
                {
                    var transactions = _transactionServices.GetTransactions(filePath);
                    transactions = _transactionServices.GetGroupedTransactionsByCategory(transactions);
                    if (transactions == null || !transactions.Any())
                    {
                        ViewBag.Error = _localizer["EmptyTransactionList"];
                        return View(compareTransactionList);
                    }

                    var transactionsViewModel = GetTransactionViewModel(transactions);

                    AppendMissingCategoryTransactions(transactionsViewModel);

                    var transactionsViewModelOrdered = transactionsViewModel.OrderBy(t => t.Category.ToString()).ToList();

                    compareTransactionList.TransactionsToCompare.Add(GetTransactionsViewModel(transactionsViewModelOrdered));
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(compareTransactionList);
                }
            }

            CompareToAdjustTransactionColor(compareTransactionList);

            return View(compareTransactionList);
        }

        private TransactionsViewModel GetTransactionsViewModel(List<TransactionViewModel> transactions)
        {
            return new TransactionsViewModel
            {
                Transactions = transactions
            };
        }

        private void AppendMissingCategoryTransactions(List<TransactionViewModel> transactions)
        {
            List<TransactionCategory> categories = GetCategories();
            
            foreach (var category in categories)
            {
                if(!transactions.Any(t => t.Category.Equals(category)))
                {
                    transactions.Add(new TransactionViewModel
                    {
                        Category = category,
                        Amount = 0
                    });
                }
            }
        }

        private List<TransactionCategory> GetCategories()
        {
            var categories = new List<TransactionCategory>();
            var categoriesName = Enum.GetNames(typeof(TransactionCategory));
            foreach (var categoryName in categoriesName)
            {
                categories.Add((TransactionCategory)Enum.Parse(typeof(TransactionCategory), categoryName));
            }

            return categories;
        }

        private void CompareToAdjustTransactionColor(CompareViewModel compareViewModel)
        {
            var oldTransactions = compareViewModel.TransactionsToCompare.First();
            foreach (var actualTransactions in compareViewModel.TransactionsToCompare.Skip(1))
            {
                oldTransactions.Transactions.ForEach(transactionToCompared =>
                {
                    var comparedTransaction = actualTransactions.Transactions.FirstOrDefault(t => t.Category.Equals(transactionToCompared.Category));

                    if (comparedTransaction != null)
                    {
                        if (comparedTransaction.Amount == transactionToCompared.Amount)
                        {
                            comparedTransaction.Color = "info";
                        }
                        else if (comparedTransaction.Amount < transactionToCompared.Amount)
                        {
                            comparedTransaction.Color = "success";
                        }
                        else
                        {
                            comparedTransaction.Color = "danger";
                        }
                    }
                });
                oldTransactions = actualTransactions;
            }
        }

        private string GetFullFilePath(string filename)
        {
            var directoryName = _configuration.GetValue<string>("CsvDirectoryName");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), directoryName, filename);
            return filePath;
        }

        private List<TransactionViewModel> GetTransactionViewModel(List<Transaction> transactions)
        {
            return transactions.Select(transaction => new Models.TransactionViewModel
            {
                Amount = transaction.Amount,
                ComptabilisationDate = transaction.ComptabilisationDate,
                Compte = transaction.Compte,
                Label = transaction.Label,
                OperationDate = transaction.OperationDate,
                Reference = transaction.Reference,
                ValueDate = transaction.ValueDate,
                Category = transaction.Category
            }).ToList();
        }

        private List<CsvFileViewModel> GetCsvList()
        {
            var directoryPath = GetCsvDirectoryPath();
            var csvFilesName = Directory.GetFiles(directoryPath, "*.csv");

            if (csvFilesName != null && csvFilesName.Any())
            {
                var csvFiles = new List<CsvFileViewModel>();
                foreach (var fileName in csvFilesName)
                {
                    var fullPath = Path.Combine(directoryPath, fileName);
                    var fileInfo = new FileInfo(fullPath);
                    csvFiles.Add(new CsvFileViewModel
                    {
                        CreatedDate = fileInfo.CreationTime,
                        FileName = fileInfo.Name,
                        FullPath = fileInfo.FullName,
                        UpdateDate = fileInfo.LastWriteTime
                    });
                }
                return csvFiles;
            }
            return new List<CsvFileViewModel>();
        }

        private string GetCsvDirectoryPath()
        {
            var directoryName = _configuration.GetValue<string>("CsvDirectoryName");
            return Path.Combine(Directory.GetCurrentDirectory(), directoryName);
        }

        // GET: Compare/Delete/fileName
        [HttpGet]
        public ActionResult Delete(string fileName)
        {
            var csvFile = new CsvFileViewModel();
            
            if (string.IsNullOrWhiteSpace(fileName))
            {
                ViewBag.Error = _localizer["EmptyFileName"];
                return View(csvFile);
            }

            var fullFilePath = Path.Combine(GetCsvDirectoryPath(), fileName);
            if(!System.IO.File.Exists(fullFilePath))
            {
                ViewBag.Error = string.Format(_localizer["FileDoesntExist"], fullFilePath);
                return View(csvFile);
            }

            return View(GetCsvFile(fullFilePath));
        }

        private CsvFileViewModel GetCsvFile(string fullFilePath)
        {
            var fileInfo = new FileInfo(fullFilePath);
            return new CsvFileViewModel
            {
                CreatedDate = fileInfo.CreationTime,
                FileName = fileInfo.Name,
                FullPath = fileInfo.FullName,
                UpdateDate = fileInfo.LastWriteTime
            };
        }

        // POST: Compare/Delete/fileName
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string fileName)
        {
            try
            {
                var fullFilePath = Path.Combine(GetCsvDirectoryPath(), fileName);
                if (string.IsNullOrWhiteSpace(fullFilePath))
                    return new NotFoundResult();

                var csvFile = GetCsvFile(fullFilePath);
                DeleteFile(csvFile.FullPath);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private void DeleteFile(string fullPath) => System.IO.File.Delete(fullPath);
    }
}