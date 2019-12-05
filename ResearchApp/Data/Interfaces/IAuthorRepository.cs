using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<DataSourceResult> GetAuthors(DataSourceRequest request);
        Task<int> CreateAuthor(AuthorViewModel model, bool updateForm = false);
        Task UpdateAuthor(AuthorViewModel model, bool updateForm = false);
    }
}
