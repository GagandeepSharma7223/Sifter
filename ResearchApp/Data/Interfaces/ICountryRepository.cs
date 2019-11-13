using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<DataSourceResult> GetCountries(DataSourceRequest request);
        Task<int> CreateCountry(CountryViewModel model);
        Task UpdateCountry(CountryViewModel model);
    }
}
