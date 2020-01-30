using Kendo.Mvc.UI;
using ResearchApp.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task Create(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(int id);

        List<DropdownOptions> GetOptions(string type, string optionCol);
        (List<dynamic>, int) GetFilterData(string type, string optionCol, DataSourceRequest request, string fieldType);
        Task<IList<TreeColumnViewModel>> GetTreeColumnsForTable(string tableName);
        AdvanceSearchViewModel SearchRecords(List<SearchParams> paramList);
        Task<List<TreeNodeViewModel>> GetTableCategories(int? id);
        string GetFKDisplayColumn(string tableName, string displayName);
    }
}
