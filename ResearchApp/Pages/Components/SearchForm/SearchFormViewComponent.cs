using Microsoft.AspNetCore.Mvc;
using ResearchApp.Data;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Pages.Components.SearchForm
{
    public class SearchFormViewComponent : ViewComponent
    {
        private readonly IWorkRepository _workRepo;
        public SearchFormViewComponent(IWorkRepository workRepo)
        {
            _workRepo = workRepo;
        }
        public IViewComponentResult Invoke(string type)
        {
            var controls = _workRepo.GetTreeColumnsForTable(type).Result;
            var formViewModel = new FormViewModel
            {
                TableColumns = controls,
                FormName = type.ToString()
            };
            return View("Default", formViewModel);
        }
    }
}