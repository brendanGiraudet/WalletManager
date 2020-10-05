using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using WalletManagerDTO;
using WalletManagerSite.Tools.Mapper;
using WalletManagerServices.Transaction;
using WalletManagerSite.Models;
using WalletManagerServices.Category;

namespace WalletManagerSite.Controllers
{
    public class TransactionController : Controller
    {
        readonly ITransactionServices _transactionServices;
        readonly IConfiguration _configuration;
        private readonly IStringLocalizer<TransactionController> _localizer;
        readonly IMapper _mapper;
        readonly ICategoryServices _categoryServices;

        public TransactionController(ITransactionServices transactionServices, IConfiguration configuration, IStringLocalizer<TransactionController> localizer, IMapper mapper, ICategoryServices categoryServices)
        {
            _transactionServices = transactionServices;
            _configuration = configuration;
            _localizer = localizer;
            _mapper = mapper;
            _categoryServices = categoryServices;
        }
        // GET: Transaction
        public ActionResult Index()
        {
            return View(GetTransactions());
        }

        // GET: Transaction/123s
        public ActionResult List(string fileName)
        {
            var transactions = new List<TransactionViewModel>();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                ViewBag.Error = _localizer["EmptyFilePath"];
                return View("Index", transactions);
            }

            var directoryPath = Tools.Directory.DirectoryTools.GetCsvDirectoryPath(_configuration);
            var filePath = Path.Combine(directoryPath, fileName);

            try
            {
                _transactionServices.LoadTransactions(filePath);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index", transactions);
        }

        private IEnumerable<TransactionViewModel> GetTransactionsViewModel(IEnumerable<Transaction> transactions)
        {
            return _mapper.MapToTransactionViewModels(transactions);
        }

        public ActionResult LoadTransactionsTable()
        {
            return PartialView("TransactionsTablePartialView", GetTransactions());
        }

        private IEnumerable<TransactionViewModel> GetTransactions()
        {
            var transactions = _transactionServices.GetTransactions();
            if (transactions != null && transactions.Any())
            {
                return GetTransactionsViewModel(transactions);
            }

            return new List<TransactionViewModel>();
        }

        [HttpPost]
        public IActionResult LoadTransactions()
        {
            var files = Request.Form.Files;
            var file = files.First();
            string message = $"{file.FileName} uploaded successfully!";

            try
            {
                _transactionServices.LoadTransactions(file.OpenReadStream());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(message);
        }

        private static void DeleteTempFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }

        [HttpGet]
        public JsonResult TransactionChart()
        {
            return Json(GetTransactionsChart());
        }

        private IEnumerable<TransactionChartViewModel> GetTransactionsChart()
        {
            var transactions = _transactionServices.GetTransactions();
            var groupedTransactionsByCategory = _transactionServices.GetGroupedTransactionsByCategory(transactions);
            var transactionsChartViewModel = _mapper.MapToTransactionsChartViewModel(groupedTransactionsByCategory);
            
            return transactionsChartViewModel;
        }

        // GET: Transaction/Details/5
        public ActionResult Details(string reference)
        {
            var transaction = new TransactionViewModel();

            if (string.IsNullOrWhiteSpace(reference))
            {
                ViewBag.Error = _localizer["EmptyReference"];
                return View(transaction);
            }

            transaction = GetTransaction(reference);
            if (transaction == null)
            {
                ViewBag.Error = _localizer["TransactionNotFound"];
                return View(transaction);
            }

            return View(transaction);
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
            var transaction = new TransactionViewModel();

            if (string.IsNullOrWhiteSpace(reference))
            {
                ViewBag.Error = _localizer["EmptyReference"];
                return View(transaction);
            }

            transaction = GetTransaction(reference);
            if (transaction == null)
            {
                ViewBag.Error = _localizer["TransactionNotFound"];
                return View(transaction);
            }

            return View(transaction);
        }

        private TransactionViewModel GetTransaction(string reference)
        {
            var transaction = _transactionServices.GetTransaction(reference);
            if (transaction != null)
            {
                var transactionViewModel = _mapper.MapToTransactionViewModel(transaction);
                
                var filePath = Tools.Directory.DirectoryTools.GetCategoryCsvFilePath(_configuration);
                var categories = _categoryServices.GetCategories(filePath);

                transactionViewModel.Categories = categories;

                return transactionViewModel;
            }
            return null;
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Compte", "ComptabilisationDate", "OperationDate", "Label", "Reference", "ValueDate", "Amount", "CategoryName")] TransactionViewModel transaction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UpdateTransaction(transaction);

                    return RedirectToAction(nameof(Index));
                }

                return View(transaction);
            }
            catch
            {
                return View(transaction);
            }
        }

        private void UpdateTransaction(TransactionViewModel transactionViewModel)
        {
            var transaction = _mapper.MapToTransactionDto(transactionViewModel);
            _transactionServices.UpdateTransaction(transaction);
        }

        // GET: Transaction/Delete/5
        public ActionResult Delete(string reference)
        {
            var transaction = new TransactionViewModel();

            if (string.IsNullOrWhiteSpace(reference))
            {
                ViewBag.Error = _localizer["EmptyReference"];
                return View(transaction);
            }

            transaction = GetTransaction(reference);
            if (transaction == null)
            {
                ViewBag.Error = _localizer["TransactionNotFound"];
                return View(new TransactionViewModel());
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletedConfirmed(string reference)
        {
            var transaction = new TransactionViewModel();
            try
            {

                if (string.IsNullOrWhiteSpace(reference))
                {
                    ViewBag.Error = _localizer["EmptyReference"];
                    return View(transaction);
                }

                _transactionServices.Delete(reference);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction(nameof(Delete), "Transaction", new { reference });
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
            var csvDirectoryPath = Tools.Directory.DirectoryTools.GetCsvDirectoryPath(_configuration);
            var filePath = Path.Combine(csvDirectoryPath, filename);
            return filePath;
        }

        public ActionResult Fusion(IEnumerable<CsvFileViewModel> csvFiles)
        {
            List<IEnumerable<Transaction>> transactionsToFusion = new List<IEnumerable<Transaction>>();

            var selectedCsvFiles = csvFiles.Where(c => c.IsChecked).ToList();
            if (selectedCsvFiles == null)
            {
                ViewBag.Error = _localizer["EmptySelectedCsvFileError"];
                return View();
            }

            if (selectedCsvFiles.Count != 2)
            {
                ViewBag.Error = _localizer["BadNumberOfCsv"];
                return View();
            }

            foreach (var selectedFile in selectedCsvFiles)
            {
                var filePath = GetFullFilePath(selectedFile.FileName);
                try
                {
                    var transactions = _transactionServices.GetTransactions(filePath);
                    if (transactions == null || !transactions.Any())
                    {
                        ViewBag.Error = _localizer["EmptyTransactionList"];
                        return View();
                    }

                    transactionsToFusion.Add(transactions);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View();
                }
            }

            var fusionedTransactions = GetFusionedTransactionList(transactionsToFusion);
            _transactionServices.SetTransactions(fusionedTransactions);

            return RedirectToAction("Index");
        }

        private IEnumerable<Transaction> GetFusionedTransactionList(IEnumerable<IEnumerable<Transaction>> transactionsToFusion)
        {
            var firstTransactionList = transactionsToFusion.FirstOrDefault();
            var secondTransactionList = transactionsToFusion.LastOrDefault();

            if (firstTransactionList != null && secondTransactionList != null)
            {
                var fusionnedTransactionList = _transactionServices.FusionTransactions(firstTransactionList, secondTransactionList);
                return fusionnedTransactionList;
            }

            return new List<Transaction>();
        }
    }
}