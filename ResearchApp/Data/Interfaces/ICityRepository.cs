using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<DataSourceResult> GetCities(DataSourceRequest request);
        Task<int> CreateCity(CityViewModel model, bool updateForm = false);
        Task UpdateCity(CityViewModel model, bool updateForm = false);
    }
}
