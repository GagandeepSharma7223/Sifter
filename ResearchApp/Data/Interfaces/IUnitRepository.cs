using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IUnitRepository : IGenericRepository<Unit>
    {
        Task<DataSourceResult> GetUnits(DataSourceRequest request);
        Task<int> CreateUnit(UnitViewModel model);
        Task UpdateUnit(UnitViewModel model);
    }
}
