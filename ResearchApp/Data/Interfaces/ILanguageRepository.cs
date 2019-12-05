using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface ILanguageRepository : IGenericRepository<Language>
    {
        Task<DataSourceResult> GetLanguages(DataSourceRequest request, bool filterRequest = false);
        Task<int> CreateLanguage(LanguageViewModel model, bool updateForm = false);
        Task UpdateLanguage(LanguageViewModel model, bool updateForm = false);
    }
}
