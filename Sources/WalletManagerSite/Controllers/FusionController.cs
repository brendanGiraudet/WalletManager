using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using WalletManagerSite.Tools.Mapper;
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
            var csvDirectoryPath = Tools.Directory.DirectoryTools.GetCsvDirectoryPath(_configuration);
            var csvFilesName = Tools.Directory.DirectoryTools.GetCsvFiles(csvDirectoryPath);

            if (csvFilesName != null && csvFilesName.Any())
            {
                var csvFiles = new List<CsvFileViewModel>();
                foreach (var fileName in csvFilesName)
                {
                    var fullPath = Path.Combine(csvDirectoryPath, fileName);
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
    }
}