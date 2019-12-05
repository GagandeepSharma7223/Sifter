using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IWorkAuthorRepository : IGenericRepository<WorkAuthor>
    {
        Task<DataSourceResult> GetWorkAuthors(DataSourceRequest request);
        Task<int> CreateWorkAuthor(WorkAuthorViewModel model, bool updateForm = false);
        Task UpdateWorkAuthor(WorkAuthorViewModel model, bool updateForm = false);
    }
}
