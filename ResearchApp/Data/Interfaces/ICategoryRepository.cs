using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<DataSourceResult> GetCategories(DataSourceRequest request);
        Task<int> CreateCategory(CategoryViewModel model);
        Task UpdateCategory(CategoryViewModel model);
    }
}
