using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WalletManagerSite.Models;

namespace WalletManagerSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = Resource.Resource.Explication_list;
            ViewData["Title"] = Resource.Resource.About;

            return View();
        }

        public IActionResult AboutTransactionIndex()
        {
            return View();
        }

        public IActionResult AboutTransactionEdit()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
