using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using WalletManagerDTO;
using WalletManagerSite.Mapper;
using WalletManagerServices.Transaction;
using WalletManagerSite.Models;

namespace WalletManagerSite.Controllers
{
    public class FusionController : Controller
    {
        readonly IConfiguration _configuration;
        readonly ITransactionServices _transactionServices;
        readonly IStringLocalizer<FusionController> _localizer;
        readonly IMapper _mapper;

        public FusionController(IConfiguration configuration, ITransactionServices transactionServices, IStringLocalizer<FusionController> localizer, IMapper mapper)
        {
            _configuration = configuration;
            _transactionServices = transactionServices;
            _localizer = localizer;
            _mapper = mapper;
        }
        // GET: Fusion
        public ActionResult Index()
        {
            return View(GetCsvList());
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
    }
}