using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class CategoryController : Controller
    {
        readonly ICategoryServices _categoryServices;
        readonly IConfiguration _configuration;
        readonly IMapper _mapper;
        readonly IStringLocalizer<CategoryController> _localizer;

        public CategoryController(
            ICategoryServices categoryServices, 
            IConfiguration configuration,
            IMapper mapper,
            IStringLocalizer<CategoryController> localizer)
        {
            _categoryServices = categoryServices;
            _configuration = configuration;
            _mapper = mapper;
            _localizer = localizer;
        }
        // GET: Index
        public ActionResult Index()
        {
            var categories = GetCategories();
            var categoryViewModels = GetCategoryViewModels(categories);
            return View(categoryViewModels);
        }

        // GET: Transaction/Delete/5
        public ActionResult Delete(string categoryName)
        {
            var categoryViewModel = new CategoryViewModel();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                ViewBag.Error = _localizer["EmptyReference"];
                return View(categoryViewModel);
            }

            categoryViewModel = GetCategory(categoryName);
            if (categoryViewModel == null)
            {
                ViewBag.Error = _localizer["CategoryNotFound"];
                return View(new CategoryViewModel());
            }

            return View(categoryViewModel);
        }

        private CategoryViewModel GetCategory(string categoryName)
        {
            var category = _categoryServices.GetCategory(categoryName);
            if (category != null)
            {
                var CategoryViewModel = _mapper.MapToCategoryViewModel(category);

                return CategoryViewModel;
            }
            return null;
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletedConfirmed(string categoryName)
        {
            var category = new CategoryViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    ViewBag.Error = _localizer["EmptyReference"];
                    return View(category);
                }

                _categoryServices.Delete(categoryName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction(nameof(Delete), "Category", new { categoryName });
            }
        }

        private IEnumerable<Category> GetCategories()
        {
            var filePath = Tools.Directory.DirectoryTools.GetCategoryCsvFilePath(_configuration);
            var categories = _categoryServices.GetCategories(filePath);
            return categories;
        }

        private IEnumerable<CategoryViewModel> GetCategoryViewModels(IEnumerable<Category> categories) => categories.Select(_mapper.MapToCategoryViewModel);

        public ActionResult Save()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string filePath = Tools.Directory.DirectoryTools.GetCategoryCsvFilePath(_configuration);
                    _categoryServices.SaveCategories(filePath);

                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}