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

        public CategoryController(
            ICategoryServices categoryServices, 
            IConfiguration configuration,
            IMapper mapper)
        {
            _categoryServices = categoryServices;
            _configuration = configuration;
            _mapper = mapper;
        }
        // GET: Compare
        public ActionResult Index()
        {
            var categories = GetCategories();
            var categoryViewModels = GetCategoryViewModels(categories);
            return View(categoryViewModels);
        }

        private IEnumerable<Category> GetCategories()
        {
            var filePath = Tools.Directory.DirectoryTools.GetCategoryCsvFilePath(_configuration);
            var categories = _categoryServices.GetCategories(filePath);
            return categories;
        }

        private IEnumerable<CategoryViewModel> GetCategoryViewModels(IEnumerable<Category> categories) => categories.Select(_mapper.MapToCategoryViewModel);
    }
}