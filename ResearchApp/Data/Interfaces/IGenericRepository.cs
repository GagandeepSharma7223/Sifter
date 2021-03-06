﻿using Kendo.Mvc.UI;
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
        DataTable PopulateSearchResult(AdvanceSearchRequest request);
        List<TreeColumnViewModel> GetMetaColumns(string tableName);
        int Create(Dictionary<string, object> model, string tableName, List<TreeColumnViewModel> columns);
        Task Update(Dictionary<string, object> model, string tableName, List<TreeColumnViewModel> columns);
        Task Destroy(string primaryKey, string tableName, int id);
        (bool, string) ValidateUniqueColumns(Dictionary<string, object> model, string tableName,
            List<TreeColumnViewModel> columns, bool updateQuery = false);
        Task<MemberViewModel> LoginUser(LoginViewModel model);
        Task SendMail(EmailViewModel model);
        Task<MemberViewModel> GetMemberDetailFromUserName(string userName);
        Task SaveSearch(CreateSearchViewModel model);
        Task<List<SearchParams>> GetSavedSearchParams(int id);
        Task<List<SavedSearchViewModel>> GetSavedSearch();
        Task<bool> ValidateSearchName(string name);
        List<dynamic> Get(string tableName, List<TreeColumnViewModel> columns, int id);
        List<string> GetBasicSearchMetaColumns();
    }
}
