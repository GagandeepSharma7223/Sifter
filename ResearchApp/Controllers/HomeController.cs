using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ResearchApp.Data;
using System.Threading.Tasks;
using System.Linq;
using ResearchApp.ViewModel;
using System.Collections.Generic;
using Kendo.Mvc.UI;

namespace ResearchApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IConfiguration Configuration;

        public HomeController(IConfiguration configuration, IAuthorRepository authorRepo)
        {
            Configuration = configuration;
            _authorRepo = authorRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult BrowseAll()
        {
            ViewBag.Search = "BrowseAll";
            return View("Search");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult LoadTreeView()
        {
            return PartialView("_PartialLoadTreeView");
        }

        [AcceptVerbs("Post")]
        public IActionResult Login(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                string validPassword = Configuration["AppSettings:ValidPassword"];
                return Json(validPassword == password.Trim());
            }
            return Json(false);
        }

        public async Task<JsonResult> Remote_Data_Binding_Get_Employees(int? id)
        {
            var result = await _authorRepo.GetTableCategories(id);
            return Json(result);
        }
        public IActionResult LoadSearchLeftPanel()
        {
            return PartialView("_PartialSearchLeftPanel");
        }
        public IActionResult LoadSearchRightPanel()
        {
            return PartialView("_PartialSearchRightPanel");
        }
    }
}
