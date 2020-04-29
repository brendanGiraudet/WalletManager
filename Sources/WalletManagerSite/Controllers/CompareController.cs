using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WalletManagerDTO;
using WalletManagerServices.Transaction;
using WalletManagerSite.Models;

namespace WalletManagerSite.Controllers
{
    public class CompareController : Controller
    {
        readonly IConfiguration _configuration;
        readonly ITransactionServices _transactionServices;

        public CompareController(IConfiguration configuration, ITransactionServices transactionServices)
        {
            _configuration = configuration;
            _transactionServices = transactionServices;
        }
        // GET: Compare
        public ActionResult Index()
        {
            return View(GetCsvList());
        }

        public ActionResult Compare(List<CsvFileViewModel> csvFiles)
        {
            var selectedCsvFiles = csvFiles.Where(c => c.IsChecked).ToList();
            if (selectedCsvFiles == null) return new NotFoundResult();

            if (selectedCsvFiles.Count != 2) return new BadRequestResult();

            string expectedFilePath = GetFullFilePath(selectedCsvFiles.First().FileName);
            string actualFilePath = GetFullFilePath(selectedCsvFiles.Last().FileName);

            List<TransactionViewModel> expectedTransactions;
            List<TransactionViewModel> actualTransactions;

            try
            {
                var transactions = _transactionServices.GetTransactions(expectedFilePath);
                expectedTransactions = GetTransactionViewModel(transactions).OrderBy(t => t.Category.ToString()).ToList();
                transactions = _transactionServices.GetTransactions(actualFilePath);
                actualTransactions = GetTransactionViewModel(transactions).OrderBy(t => t.Category.ToString()).ToList();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }

            if (expectedTransactions == null || actualTransactions == null) return new NotFoundResult();

            CompareToAdjustTransactionColor(expectedTransactions, actualTransactions);

            return View(GetCompareViewModel(expectedTransactions, actualTransactions));
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

        // GET: Transaction/Edit/fileName
        [HttpGet]
        public ActionResult Edit(string fileName)
        {
            var fullFilePath = Path.Combine(GetCsvDirectoryPath(), fileName);
            if (string.IsNullOrWhiteSpace(fullFilePath))
                return new NotFoundResult();

            try
            {
                _transactionServices.LoadTransactions(fullFilePath);
                return RedirectToAction("Index", "Transaction");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Transaction/Delete/fileName
        [HttpGet]
        public ActionResult Delete(string fileName)
        {
            var fullFilePath = Path.Combine(GetCsvDirectoryPath(), fileName);
            if (string.IsNullOrWhiteSpace(fullFilePath))
                return new NotFoundResult();

            var csvFile = GetCsvFile(fullFilePath);
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

        // POST: Transaction/Delete/fileName
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