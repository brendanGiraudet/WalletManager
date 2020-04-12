using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalletManagerServices.Transaction;

namespace WalletManagerSite.Controllers
{
    public class TransactionController : Controller
    {
        readonly ITransactionServices _transactionServices;

        public TransactionController(ITransactionServices transactionServices)
        {
            _transactionServices = transactionServices;
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
            var transactions = _transactionServices.GetTransactions();
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
            var transactions = _transactionServices.GetTransactions();

            return Json(transactions);
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
            return View(GetTransaction(reference));
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
            return new Models.TransactionViewModel();
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
    }
}