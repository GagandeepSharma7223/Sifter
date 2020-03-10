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
        //AdvanceSearchViewModel SearchRecords(List<SearchParams> paramList);
        Task<List<TreeNodeViewModel>> GetTableCategories(int? id);
        string GetFKDisplayColumn(string tableName, string displayName);
        DataTable PopulateSearchResult(DataTable searchTable, AdvanceSearchRequest request);
        List<TreeColumnViewModel> GetMetaColumns(string tableName);
        int Create(Dictionary<string, object> model, string tableName, List<TreeColumnViewModel> columns);
        Task Update(Dictionary<string, object> model, string tableName, List<TreeColumnViewModel> columns);
        Task Destroy(string primaryKey, string tableName, int id);
        (bool, string) ValidateUniqueColumns(Dictionary<string, object> model, string tableName,
            List<TreeColumnViewModel> columns, bool updateQuery = false);
        Task<bool> LoginUser(LoginViewModel model);
        Task SendMail(EmailViewModel model);
        Task<(string, string)> GetMemberDetailFromUserName(string userName);
    }
}
