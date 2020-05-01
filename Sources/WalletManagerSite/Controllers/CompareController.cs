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
            List<TransactionViewModel> expectedTransactions = new List<TransactionViewModel>();
            List<TransactionViewModel> actualTransactions = new List<TransactionViewModel>();

            var selectedCsvFiles = csvFiles.Where(c => c.IsChecked).ToList();
            if (selectedCsvFiles == null)
            {
                ViewBag.Error = _localizer["EmptySelectedCsvFileError"];
                return View(GetCompareViewModel(expectedTransactions, actualTransactions));
            }

            if (selectedCsvFiles.Count != 2)
            {
                ViewBag.Error = _localizer["EmptySelectedCsvFileError"];
                return View(GetCompareViewModel(expectedTransactions, actualTransactions));
            }

            string expectedFilePath = GetFullFilePath(selectedCsvFiles.First().FileName);
            string actualFilePath = GetFullFilePath(selectedCsvFiles.Last().FileName);

            try
            {
                var transactions = _transactionServices.GetTransactions(expectedFilePath);
                expectedTransactions = GetTransactionViewModel(transactions);
                transactions = _transactionServices.GetTransactions(actualFilePath);
                actualTransactions = GetTransactionViewModel(transactions);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(GetCompareViewModel(expectedTransactions, actualTransactions));
            }

            if (expectedTransactions == null || actualTransactions == null)
            {
                ViewBag.Error = _localizer["EmptyTransactionList"];
                return View(GetCompareViewModel(expectedTransactions, actualTransactions));
            }

            AppendMissingCategoryTransactions(expectedTransactions);
            AppendMissingCategoryTransactions(actualTransactions);

            CompareToAdjustTransactionColor(expectedTransactions, actualTransactions);

            expectedTransactions = expectedTransactions.OrderBy(t => t.Category.ToString()).ToList();
            actualTransactions = actualTransactions.OrderBy(t => t.Category.ToString()).ToList();

            return View(GetCompareViewModel(expectedTransactions, actualTransactions));
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

        private void CompareToAdjustTransactionColor(List<TransactionViewModel> expectedTransactions, List<TransactionViewModel> actualTransactions)
        {
            expectedTransactions.ForEach(transactionToCompared =>
            {
                var comparedTransaction = actualTransactions.FirstOrDefault(t => t.Category.Equals(transactionToCompared.Category));
                
                if (comparedTransaction != null)
                {
                    if (comparedTransaction.Amount == transactionToCompared.Amount)
                    {
                        comparedTransaction.Color = "info";
                        transactionToCompared.Color = "info";
                    }
                    else if (comparedTransaction.Amount < transactionToCompared.Amount)
                    {
                        comparedTransaction.Color = "success";
                        transactionToCompared.Color = "danger";
                    }
                    else
                    {
                        comparedTransaction.Color = "danger";
                        transactionToCompared.Color = "success";
                    }
                }
            });
        }

        private string GetFullFilePath(string filename)
        {
            var directoryName = _configuration.GetValue<string>("CsvDirectoryName");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), directoryName, filename);
            return filePath;
        }

        private CompareViewModel GetCompareViewModel(List<TransactionViewModel> expectedTransactions, List<TransactionViewModel> actualTransactions)
        {
            return new CompareViewModel
            {
                ActualTransactions = actualTransactions,
                ExpectedTransactions = expectedTransactions
            };
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
            csvFile = GetCsvFile(fullFilePath);

            return View(csvFile);
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