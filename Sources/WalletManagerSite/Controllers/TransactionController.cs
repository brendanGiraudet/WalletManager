using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WalletManagerServices.Transaction;
using WalletManagerSite.Models;

namespace WalletManagerSite.Controllers
{
    public class TransactionController : Controller
    {
        readonly ITransactionServices _transactionServices;
        readonly IConfiguration _configuration;

        public TransactionController(ITransactionServices transactionServices, IConfiguration configuration)
        {
            _transactionServices = transactionServices;
            _configuration = configuration;
        }
        // GET: Transaction
        public ActionResult Index()
        {
            return View(GetTransactions());
        }

        public ActionResult LoadTransactionsTable()
        {
            return PartialView("TransactionsTablePartialView", GetTransactions());
        }

        private List<Models.TransactionViewModel> GetTransactions()
        {
            var transactions = _transactionServices.GetGroupedTransactionsByLabel();
            if (transactions != null && transactions.Any())
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
            return new List<Models.TransactionViewModel>();
        }

        [HttpPost]
        public IActionResult LoadTransactions()
        {
            var files = Request.Form.Files;
            var file = files.First();
            string filePath = Path.Combine(Path.GetTempPath(), file.FileName);
            string message = $"{filePath} uploaded successfully!";

            try
            {
                CopyContentInTempFile(file, filePath);
                _transactionServices.LoadTransactions(filePath);
            }
            catch (System.Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(message);
        }

        private static void CopyContentInTempFile(IFormFile file, string filePath)
        {
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        [HttpGet]
        public JsonResult TransactionChart()
        {
            return Json(GetTransactionsChart());
        }

        private object GetTransactionsChart()
        {
            var debitTransactions = _transactionServices.GetDebitTransactions();
            var groupedTransactionsByCategory = _transactionServices.GetGroupedTransactionsByCategory(debitTransactions);
            return groupedTransactionsByCategory.Select(transaction => new TransactionChartViewModel
            {
                Amount = transaction.Amount * -1, // google piechart should only have positif amount
                Category = transaction.Category.ToString()
            }).ToList();
        }

        // GET: Transaction/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Transaction/Edit/5GX5
        public ActionResult Edit(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return new NotFoundResult();
            }
            var transaction = GetTransaction(reference);
            if (transaction == null)
            {
                return new NotFoundResult();
            }

            return View(transaction);
        }
        private Models.TransactionViewModel GetTransaction(string reference)
        {
            var transaction = _transactionServices.GetTransaction(reference);
            if (transaction != null)
            {
                return new Models.TransactionViewModel
                {
                    Amount = transaction.Amount,
                    ComptabilisationDate = transaction.ComptabilisationDate,
                    Compte = transaction.Compte,
                    Label = transaction.Label,
                    OperationDate = transaction.OperationDate,
                    Reference = transaction.Reference,
                    ValueDate = transaction.ValueDate,
                    Category = transaction.Category
                };
            }
            return null;
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Compte", "ComptabilisationDate", "OperationDate", "Label", "Reference", "ValueDate", "Amount", "Category")] TransactionViewModel transaction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UpdateTransaction(transaction);

                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        private void UpdateTransaction(TransactionViewModel transactionViewModel)
        {
            var transaction = new WalletManagerDTO.Transaction
            {
                Amount = transactionViewModel.Amount,
                Category = transactionViewModel.Category,
                ComptabilisationDate = transactionViewModel.ComptabilisationDate,
                Compte = transactionViewModel.Compte,
                Label = transactionViewModel.Label,
                OperationDate = transactionViewModel.OperationDate,
                Reference = transactionViewModel.Reference,
                ValueDate = transactionViewModel.ValueDate
            };
            _transactionServices.UpdateTransaction(transaction);
        }

        // GET: Transaction/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Transaction/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Transaction/Save
        public ActionResult Save()
        {
            return View();
        }

        // POST: Transaction/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind("FileName")] SaveTransactionsViewModel saveTransactionsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string filePath = GetFullFilePath(saveTransactionsViewModel.FileName);
                    _transactionServices.SaveTransactionsIntoCsvFile(filePath);

                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        private string GetFullFilePath(string filename)
        {
            var directoryName = _configuration.GetValue<string>("CsvDirectoryName");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), directoryName, filename);
            return filePath;
        }
    }
}