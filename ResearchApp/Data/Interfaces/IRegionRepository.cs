using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IRegionRepository : IGenericRepository<Region>
    {
        Task<DataSourceResult> GetRegions(DataSourceRequest request);
        Task<int> CreateRegion(RegionViewModel model, bool updateForm = false);
        Task UpdateRegion(RegionViewModel model, bool updateForm = false);
    }
}
