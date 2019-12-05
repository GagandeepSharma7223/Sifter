using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        Task<DataSourceResult> GetWorks(DataSourceRequest request, bool filterRequest = false);
        Task<int> CreateWork(WorkViewModel model, bool updateForm = false);
        Task UpdateWork(WorkViewModel model, bool updateForm = false);
    }
}
