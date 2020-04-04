using System.Collections.Generic;
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

        private List<Models.TransactionViewModel> GetTransactions()
        {
            var transactions = _transactionServices.GetTransactions();
            if(transactions != null && transactions.Any())
            {
                return transactions.Select(transaction => new Models.TransactionViewModel
                {
                    Amount = transaction.Amount,
                    ComptabilisationDate = transaction.ComptabilisationDate,
                    Compte = transaction.Compte,
                    Label = transaction.Label,
                    OperationDate = transaction.OperationDate,
                    Reference = transaction.Reference,
                    ValueDate = transaction.ValueDate
                }).ToList();
            }
            return new List<Models.TransactionViewModel>();
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

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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