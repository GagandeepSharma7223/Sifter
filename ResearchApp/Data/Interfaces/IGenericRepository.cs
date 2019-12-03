using Kendo.Mvc.UI;
using ResearchApp.ViewModel;
using System.Collections.Generic;
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
        List<dynamic> GetFilterData(string type, string optionCol, int page, int pageSize, string fieldType);
        Task<IList<TreeColumnViewModel>> GetTreeColumnsForTable(string tableName);
    }
}
