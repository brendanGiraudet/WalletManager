using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using WalletManagerDTO;
using WalletManagerDTO.Enumerations;
using WalletManagerSite.Tools.Mapper;
using WalletManagerServices.Transaction;
using WalletManagerSite.Models;

namespace WalletManagerSite.Controllers
{
    public class CompareController : Controller
    {
        readonly IConfiguration _configuration;
        readonly ITransactionServices _transactionServices;
        readonly IStringLocalizer<CompareController> _localizer;
        readonly IMapper _mapper;

        public CompareController(IConfiguration configuration, ITransactionServices transactionServices, IStringLocalizer<CompareController> localizer, IMapper mapper)
        {
            _configuration = configuration;
            _transactionServices = transactionServices;
            _localizer = localizer;
            _mapper = mapper;
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

                    var transactionViewModels = GetTransactionViewModels(transactions);

                    AppendMissingCategoryTransactions(transactionViewModels);

                    var transactionsViewModelOrdered = transactionViewModels.OrderBy(t => t.Category.ToString()).ToList();

                    UnsignAmount(transactionsViewModelOrdered);

                    var transactionsViewModel = GetTransactionsViewModel(transactionsViewModelOrdered);

                    compareTransactionList.TransactionsToCompare.Add(transactionsViewModel);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(compareTransactionList);
                }
            }

            compareTransactionList.TransactionsToCompare = OrderTransactionsByDate(compareTransactionList);
            CompareToAdjustTransactionColor(compareTransactionList);

            return View(compareTransactionList);
        }

        private static List<TransactionsViewModel> OrderTransactionsByDate(CompareViewModel compareTransactionList)
        {
            return compareTransactionList.TransactionsToCompare.OrderBy(t => t.Date).ToList();
        }

        private static void UnsignAmount(List<TransactionViewModel> transactionsViewModelOrdered)
        {
            transactionsViewModelOrdered.ForEach(t => { if (t.Amount < 0) t.Amount *= -1; });
        }

        private TransactionsViewModel GetTransactionsViewModel(List<TransactionViewModel> transactionsViewModel)
        {
            return _mapper.MapToTransactionsViewModel(transactionsViewModel);
        }

        private void AppendMissingCategoryTransactions(List<TransactionViewModel> transactions)
        {
            List<TransactionCategory> categories = GetCategories();
            var firstTransaction = transactions.FirstOrDefault();

            foreach (var category in categories)
            {
                if (!transactions.Any(t => t.Category.Equals(category)))
                {
                    transactions.Add(new TransactionViewModel
                    {
                        Category = category,
                        Amount = 0,
                        OperationDate = firstTransaction != null ? firstTransaction.OperationDate : DateTime.Now
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
            var csvDirectoryPath = Tools.Directory.DirectoryTools.GetCsvDirectoryPath(_configuration);
            var filePath = Path.Combine(csvDirectoryPath, filename);
            return filePath;
        }

        private List<TransactionViewModel> GetTransactionViewModels(List<Transaction> transactions)
        {
            return _mapper.MapToTransactionViewModels(transactions).ToList();
        }

        private List<CsvFileViewModel> GetCsvList()
        {
            var directoryPath = Tools.Directory.DirectoryTools.GetCsvDirectoryPath(_configuration);
            var csvFilesName = Tools.Directory.DirectoryTools.GetCsvFiles(directoryPath);

            if (csvFilesName != null && csvFilesName.Any())
            {
                var csvFiles = new List<CsvFileViewModel>();
                foreach (var csvFile in csvFilesName)
                {
                    var fileInfo = new FileInfo(csvFile);
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

            var csvDirectoryPath = Tools.Directory.DirectoryTools.GetCsvDirectoryPath(_configuration);
            var fullFilePath = Path.Combine(csvDirectoryPath, fileName);
            if (!System.IO.File.Exists(fullFilePath))
            {
                ViewBag.Error = string.Format(_localizer["FileDoesntExist"], fullFilePath);
                return View(csvFile);
            }

            return View(GetCsvFile(fullFilePath));
        }

        private CsvFileViewModel GetCsvFile(string fullFilePath)
        {
            var fileInfo = new FileInfo(fullFilePath);
            return _mapper.MapToCsvFileViewModel(fileInfo);
        }

        // POST: Compare/Delete/fileName
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string fileName)
        {
            var csvFile = new CsvFileViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    ViewBag.Error = _localizer["EmptyFileName"];
                    return View(csvFile);
                }

                var csvDirectoryPath = Tools.Directory.DirectoryTools.GetCsvDirectoryPath(_configuration);
                var fullFilePath = Path.Combine(csvDirectoryPath, fileName);
                if (!System.IO.File.Exists(fullFilePath))
                {
                    ViewBag.Error = string.Format(_localizer["FileDoesntExist"], fullFilePath);
                    return View(csvFile);
                }

                DeleteFile(fullFilePath);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(csvFile);
            }
        }

        private void DeleteFile(string fullPath) => System.IO.File.Delete(fullPath);
    }
}