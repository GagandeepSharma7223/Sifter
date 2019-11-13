using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<DataSourceResult> GetAuthors(DataSourceRequest request);
        Task<int> CreateAuthor(AuthorViewModel model);
        Task UpdateAuthor(AuthorViewModel model);
    }
}
