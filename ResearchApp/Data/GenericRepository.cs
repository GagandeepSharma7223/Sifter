using Kendo.Mvc;
using Kendo.Mvc.UI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ResearchApp.Data.Enum;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using ResearchApp.Extension;
namespace ResearchApp.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class
    {
        public readonly SifterContext _dbContext;
        private readonly IMemoryCache _cache;

        public GenericRepository(SifterContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _cache = memoryCache;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task Create(TEntity entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                _cache.Remove(GetClientCacheKey(typeof(TEntity).Name, CacheTypes.DropdownOptions));
            }
            catch (Exception e)
            {
                //throw;
            }
        }

        public async Task Update(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();
                _cache.Remove(GetClientCacheKey(typeof(TEntity).Name, CacheTypes.DropdownOptions));
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public async Task Delete(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public List<DropdownOptions> GetOptions(string type, string optionCol)
        {
            var result = new List<DropdownOptions>();
            var groupedItems = _dbContext.Query($"ResearchApp.Models.{type}")
                .Select($"new ({type}Id as Id,{optionCol} as Option)").ToDynamicList();

            result = groupedItems.Select(x => new DropdownOptions
            {
                Id = x.Id,
                Option = x.Option
            }).Where(x => !string.IsNullOrEmpty(x.Option)).OrderBy(x => x.Option).ToList();
            result = result.GroupBy(x => x.Option).Select(g => g.First()).ToList();

            return result;
        }

        public (List<dynamic>, int) GetFilterData(string type, string optionCol, DataSourceRequest request, string fieldType)
        {
            FilterDescriptor descriptor =
                   request.Filters.Count > 0 ? request.Filters.ElementAt(0) as FilterDescriptor : null;
            if (fieldType != "object")
            {
                string filterQuery = descriptor != null ? $"{optionCol} like '%{descriptor.Value}%'" : "1=1";
                string query = $"SELECT DISTINCT {optionCol} FROM {type} WHERE {optionCol}!=@a and {filterQuery}" +
                    $"ORDER BY {optionCol} OFFSET { (request.Page - 1) * request.PageSize } ROWS FETCH NEXT { request.PageSize } ROWS ONLY";
                string queryCount = $"SELECT COUNT(DISTINCT {optionCol}) FROM {type} WHERE {optionCol}!=@a and {filterQuery}";
                var result = _dbContext.DynamicListFromSql(query,
                    new Dictionary<string, object> { { "a", string.Empty } }).ToList();
                var resultCount = _dbContext.DynamicListFromSql(queryCount,
                    new Dictionary<string, object> { { "a", string.Empty } }).ToList();
                return (result, resultCount[0]);
            }
            else
            {
                var sqltblname = _dbContext.DynamicListFromSql($"SELECT FKTable  FROM TreeColumn WHERE TableName=@a and  DisplayName = @b", new Dictionary<string, object> { { "a", type }, { "b", optionCol } }).FirstOrDefault();
                var colName = _dbContext.DynamicListFromSql($"SELECT FKDisplayCol  FROM TreeColumn WHERE TableName=@a and  DisplayName = @b", new Dictionary<string, object> { { "a", type }, { "b", optionCol } }).FirstOrDefault();
                string filterQuery = descriptor != null ? $"{colName} like '%{descriptor.Value}%'" : "1=1";
                string query = $"SELECT MAX({sqltblname}ID) as {sqltblname}ID ,{colName} FROM {sqltblname} where {colName} != @a and {filterQuery} " +
                      $" GROUP BY {colName} ORDER BY {colName}  OFFSET { (request.Page - 1) * request.PageSize } ROWS FETCH NEXT { request.PageSize } ROWS ONLY";
                string queryCount = $"SELECT COUNT(DISTINCT {colName}) FROM {sqltblname} WHERE {colName}!=@a and {filterQuery}";
                var result = _dbContext.DynamicListFromSql(query, new Dictionary<string, object> { { "a", string.Empty } }, fieldType).ToList();
                var resultCount = _dbContext.DynamicListFromSql(queryCount,
                    new Dictionary<string, object> { { "a", string.Empty } }).ToList();
                return (result, resultCount[0]);
            }
        }

        public async Task<List<StringCompareRequest>> ModifyFilters(TEntity entity, IList<IFilterDescriptor> filters, string repoTable)
        {
            var listOfStringCompare = new List<StringCompareRequest>();
            if (filters.Any())
            {
                for (int i = filters.Count() - 1; i > -1; i--)
                {
                    var descriptor = filters.ElementAt(i) as FilterDescriptor;
                    if (descriptor != null)
                    {
                        // Filter For Greater Than and less than 
                        if (descriptor.Operator == FilterOperator.IsGreaterThanOrEqualTo ||
                            descriptor.Operator == FilterOperator.IsLessThanOrEqualTo ||
                            descriptor.Operator == FilterOperator.IsGreaterThan ||
                            descriptor.Operator == FilterOperator.IsLessThan)
                        {
                            int value = Convert.ToInt32(descriptor.Value);
                            var fkColumn = await GetTreeColumns().Where(x => x.TableName == repoTable && x.DisplayName == descriptor.Member).Select(x => x.FkdisplayCol).FirstOrDefaultAsync();
                            if (!string.IsNullOrEmpty(fkColumn))
                            {
                                // Get String Value From Integer Value
                                var dropdownText = _dbContext.DynamicListFromSql($"SELECT {fkColumn}  FROM {descriptor.Member} WHERE {descriptor.Member}Id=@0", new Dictionary<string, object> { { "0", value } }).FirstOrDefault();
                                if (!string.IsNullOrEmpty(dropdownText))
                                {
                                    listOfStringCompare.Add(new StringCompareRequest
                                    {
                                        Entity = descriptor.Member,
                                        Value = dropdownText,
                                        Operator = descriptor.Operator,
                                        FKColumn = fkColumn
                                    });
                                }
                                filters.RemoveAt(i);
                            }
                        }
                        else
                        {
                            PropertyInfo propInfo = GetPropertyType(entity, descriptor.Member);
                            if (propInfo.PropertyType == typeof(DropdownOptions))
                            {
                                descriptor.Member = descriptor.Member + "Id";
                            }
                        }
                    }
                    else if (filters.ElementAt(i) is CompositeFilterDescriptor)
                    {
                        var list = await ModifyFilters(entity, ((CompositeFilterDescriptor)filters.ElementAt(i)).FilterDescriptors, repoTable);
                        if (list.Any())
                        {
                            listOfStringCompare.AddRange(list);
                        }
                    }
                }
            }
            return listOfStringCompare;
        }

        public async Task<List<StringCompareRequest>> ModifyFilters(TEntity entity, IList<FilterDescriptor> filters, string repoTable)
        {
            var listOfStringCompare = new List<StringCompareRequest>();
            if (filters.Any())
            {
                for (int i = filters.Count() - 1; i > -1; i--)
                {
                    var descriptor = filters.ElementAt(i) as FilterDescriptor;
                    if (descriptor != null)
                    {
                        // Filter For Greater Than and less than 
                        if (descriptor.Operator == FilterOperator.IsGreaterThanOrEqualTo ||
                            descriptor.Operator == FilterOperator.IsLessThanOrEqualTo ||
                            descriptor.Operator == FilterOperator.IsGreaterThan ||
                            descriptor.Operator == FilterOperator.IsLessThan)
                        {
                            int value = Convert.ToInt32(descriptor.Value);
                            //var fkColumn = await GetTreeColumns().Where(x => x.TableName == repoTable && x.DisplayName == descriptor.Member).Select(x => x.FkdisplayCol).FirstOrDefaultAsync();
                            // Get String Value From Integer Value
                            //var dropdownText = _dbContext.DynamicListFromSql($"SELECT {fkColumn}  FROM {descriptor.Member} WHERE {descriptor.Member}Id=@0", new Dictionary<string, object> { { "0", value } }).FirstOrDefault();
                            //if (!string.IsNullOrEmpty(dropdownText))
                            //{
                            //    listOfStringCompare.Add(new StringCompareRequest
                            //    {
                            //        Entity = descriptor.Member,
                            //        Value = dropdownText,
                            //        Operator = descriptor.Operator,
                            //        FKColumn = fkColumn
                            //    });
                            //}
                            filters.RemoveAt(i);
                        }
                        //else
                        //{
                        //    PropertyInfo propInfo = GetPropertyType(entity, descriptor.Member);
                        //    if (propInfo.PropertyType == typeof(DropdownOptions))
                        //    {
                        //        descriptor.Member = descriptor.Member + "Id";
                        //    }
                        //}
                    }
                    //else if (filters.ElementAt(i) is CompositeFilterDescriptor)
                    //{
                    //    var list = await ModifyFilters(entity, ((CompositeFilterDescriptor)filters.ElementAt(i)).FilterDescriptors, repoTable);
                    //    if (list.Any())
                    //    {
                    //        listOfStringCompare.AddRange(list);
                    //    }
                    //}
                }
            }
            return listOfStringCompare;
        }


        private static PropertyInfo GetPropertyType(TEntity entity, string propName)
        {
            Type type = entity.GetType();
            PropertyInfo propInfo = type.GetProperty(propName);
            return propInfo;
        }

        public string GetOperatorSymbol(FilterOperator op)
        {
            switch (op)
            {
                case FilterOperator.IsLessThan:
                    return "<";
                case FilterOperator.IsLessThanOrEqualTo:
                    return "<=";
                case FilterOperator.IsGreaterThanOrEqualTo:
                    return ">=";
                case FilterOperator.IsGreaterThan:
                    return ">";
                default:
                    return ">";
            }
        }

        public IQueryable<MetaColumn> GetTreeColumns()
        {
            return _dbContext.MetaColumn;
        }

        public async Task<IList<TreeColumnViewModel>> GetTreeColumnsForTable(string tableName)
        {
            return await _dbContext.MetaColumn.Where(x => x.TableName == tableName).OrderBy(x => x.ColSeq)
                .Select(x => new TreeColumnViewModel
                {
                    ColSeq = x.ColSeq,
                    ColumnName = x.ColumnName,
                    Display = x.IsDisplayed,
                    IDColumn = x.Idcolumn ?? false,
                    DisplayName = x.DisplayName,
                    Editable = x.IsEditable,
                    FkdisplayCol = x.FkdisplayCol,
                    FkjoinCol = x.FkjoinCol,
                    Fktable = x.Fktable,
                    TableName = x.TableName,
                    IsEditable = x.IsEditable ?? false,
                    IsRequired = x.IsRequired ?? false,
                    IsUnique = x.IsUnique ?? false,
                    Type = !string.IsNullOrEmpty(x.Fktable) ? "dropdown" : x.ColType
                }).ToListAsync();
        }

        public async Task<List<TreeNodeViewModel>> GetTableCategories(int? id)
        {
            var result = new List<TreeNodeViewModel>();
            result = await _dbContext.MetaCategory.Select(x => new TreeNodeViewModel
            {
                id = x.MetaCategoryId,
                Text = x.Name,
                hasChildren = true,
                expanded = true
            }).ToListAsync();
            if (id.HasValue)
            {
                result = await _dbContext.MetaTable.OrderBy(x => x.TableSeq)
                                    .Where(x => x.MetaCategoryId == id).Select(x => new TreeNodeViewModel
                                    {
                                        Text = x.DisplayName,
                                        tableName = x.TableName
                                    }).ToListAsync();
            }
            return result;
        }

        public void ApplyFilter(DataSourceRequest request)
        {
            if (request.Sorts.Any())
            {
                foreach (SortDescriptor sortDescriptor in request.Sorts)
                {
                    switch (sortDescriptor.Member)
                    {
                        case "Author":
                            sortDescriptor.Member = "Author.FullName";
                            break;
                        case "Translator":
                            sortDescriptor.Member = "Author.FullName";
                            break;
                        case "Editor":
                            sortDescriptor.Member = "Author.FullName";
                            break;
                        case "City":
                            sortDescriptor.Member = "City.Name";
                            break;
                        case "Publisher":
                            sortDescriptor.Member = "Publisher.Name";
                            break;
                        case "Language":
                            sortDescriptor.Member = "Language.Name";
                            break;
                        case "Region":
                            sortDescriptor.Member = "Region.Name";
                            break;
                        case "Country":
                            sortDescriptor.Member = "Country.Name";
                            break;
                        case "Category":
                            sortDescriptor.Member = "Category.Name";
                            break;
                        case "Work":
                            sortDescriptor.Member = "Work.Title";
                            break;
                    }
                }
            }
        }

        public string GetClientCacheKey(string entityName, CacheTypes cacheType)
        {
            switch (cacheType)
            {
                case CacheTypes.DropdownOptions:
                    return $"{entityName}Options";
                default:
                    return string.Empty;
            }
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        //public void SearchRecords<T>(List<SearchParams> paramList)
        //{
        //    var table = paramList.ToDataTable();

        //    ExecuteProcedure(table);
        //}

        //private void ExecuteProcedure<T>(DataTable table)
        //{
        //    var result = new AdvanceSearchViewModel();
        //    try
        //    {
        //        //PopulateSearchResult(table, request);
        //        //switch (request.GridType)
        //        //{
        //        //    case GridTypes.VAuthor:
        //        //        var res = new AdvanceSearchResult<VAuthor>
        //        //        {
        //        //            Result = PopulateSearchResult<VAuthor>(table, request).ToList<VAuthor>(),
        //        //            Columns = GetMetaColumns("vAuthor")
        //        //        };
        //        //        break;
        //        //}

        //        using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
        //        {
        //            connection.Open();
        //            using (SqlCommand command = connection.CreateCommand())
        //            {
        //                command.CommandText = "dbo.advancedSearch";
        //                command.CommandType = System.Data.CommandType.StoredProcedure;

        //                SqlParameter parameter = command.Parameters
        //                                  .AddWithValue("@params", table);
        //                parameter.SqlDbType = SqlDbType.Structured;
        //                parameter.TypeName = "dbo.SearchParams";
        //                SqlDataAdapter adapter = new SqlDataAdapter(command);
        //                DataSet ds = new DataSet();
        //                adapter.Fill(ds);
        //                var res = ds.Tables[0];
        //                result.Authors = ds.Tables[0].ToList<VAuthor>();
        //                result.Works = ds.Tables[1].ToList<VWork>();
        //                result.Units = ds.Tables[2].ToList<VUnit>();
        //            }

        //            result.AuthorColumns = GetMetaColumns("vAuthor");
        //            result.WorkColumns = GetMetaColumns("vWork");
        //            result.UnitColumns = GetMetaColumns("vUnit");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public DataTable PopulateSearchResult(DataTable searchTable, AdvanceSearchRequest request)
        {
            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.advancedSearchPage";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    List<SqlParameter> parameters = new List<SqlParameter>()
                     {
                         new SqlParameter("@params", SqlDbType.Structured) {Value = searchTable, TypeName= "dbo.SearchParams"},
                         new SqlParameter("@outputType", SqlDbType.VarChar, 20) {Value = request.SearchType},
                         new SqlParameter("@countOnly", SqlDbType.Bit) {Value = request.CountOnly},
                         new SqlParameter("@offsetRows", SqlDbType.Int) {Value = request.PageNumber * request.PageSize},
                         new SqlParameter("@fetchRows", SqlDbType.Int) {Value = request.PageSize},
                         new SqlParameter("@sortByCol", SqlDbType.VarChar, 60) {Value = request.SortField},
                         new SqlParameter("@sortByOrder", SqlDbType.VarChar, 10) {Value = request.SortDirection}
                     };

                    command.Parameters.AddRange(parameters.ToArray());
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        public List<TreeColumnViewModel> GetMetaColumns(string tableName)
        {
            return _dbContext.MetaColumn.Where(x => x.TableName == tableName
            && x.IsDisplayed == true)
                .OrderBy(x => x.ColSeq).Select(x => new TreeColumnViewModel
                {
                    DisplayName = x.DisplayName,
                    ColumnName = x.ColumnName,
                    IDColumn = x.Idcolumn ?? false
                }).ToList();
        }

        public string GetFKDisplayColumn(string tableName, string displayName)
        {
            return _dbContext.DynamicListFromSql($"SELECT FKDisplayCol  FROM TreeColumn WHERE TableName=@a and  DisplayName = @b",
                new Dictionary<string, object> { { "a", tableName }, { "b", displayName } }).FirstOrDefault();
        }
    }
}


public class Helper
{
    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}