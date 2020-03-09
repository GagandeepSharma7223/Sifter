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

        public int Create(Dictionary<string, object> model, string tableName, List<TreeColumnViewModel> columns)
        {
            var columnsToAdd = columns.Where(x => x.IsEditable == true && !x.IDColumn).Select(x => x.ColumnName);
            var keysToRemove = model.Keys.Where(x => !columnsToAdd.Contains(x)).ToList();
            keysToRemove.ForEach(x => model.Remove(x));
            string columnNames = string.Join(", ", model.Select(x => x.Key));
            var keys = model.Select(x => x.Key);
            string indexes = string.Empty;
            for (int i = 0; i < keys.Count(); i++)
            {
                indexes += "@" + keys.ElementAt(i);
                if (i < keys.Count() - 1)
                    indexes += ", ";
            }
            string query = $"INSERT INTO {tableName} ({columnNames}) Output Inserted.{tableName}ID VALUES ({indexes})";
            return _dbContext.ExecuteScalarFromSql(query, model);
        }

        public (bool, string) ValidateUniqueColumns(Dictionary<string, object> model,
            string tableName, List<TreeColumnViewModel> columns, bool updateQuery = false)
        {
            bool hasDuplicateValue = false;
            string duplicateColumn = string.Empty, query = string.Empty;
            var uniqueColumns = columns.Where(x => x.IsUnique).Select(x => x.ColumnName);
            if (uniqueColumns.Any())
            {
                foreach (var column in uniqueColumns)
                {
                    var item = model.Where(x => x.Key == column).FirstOrDefault();
                    if (item.Value != null && item.Value.ToString() != string.Empty)
                    {
                        if (updateQuery)
                        {
                            string idColumn = columns.Where(x => x.IDColumn).Select(x => x.ColumnName).FirstOrDefault();
                            int id = Convert.ToInt32(model.Where(x => x.Key == idColumn).Select(x => x.Value).FirstOrDefault());
                            query = $"SELECT COUNT({item.Key}) FROM {tableName} WHERE LOWER({item.Key}) = '{item.Value.ToString().Trim().ToLower()}' and {idColumn} != {id}";
                        }
                        else
                        {
                            query = $"SELECT COUNT({item.Key}) FROM {tableName} WHERE {item.Key} = '{item.Value}'";
                        }
                        var resultCount = _dbContext.DynamicListFromSql(query,
                        new Dictionary<string, object> { }).ToList();
                        var count = Convert.ToInt32(resultCount[0]);
                        if (count > 0)
                        {
                            duplicateColumn = item.Key;
                            hasDuplicateValue = true;
                        }
                        if (hasDuplicateValue)
                        {
                            break;
                        }
                    }
                }
            }
            return (hasDuplicateValue, duplicateColumn);
        }

        public async Task Update(Dictionary<string, object> model, string tableName, List<TreeColumnViewModel> columns)
        {
            var columnsToUpdate = columns.Where(x => x.IsEditable == true).Select(x => x.ColumnName);
            var primaryKey = model.Where(x => x.Key == columns.Where(x => x.IDColumn)
                .Select(x => x.ColumnName).FirstOrDefault())
                    .Select(x => x.Value).FirstOrDefault();
            var keysToRemove = model.Keys.Where(x => !columnsToUpdate.Contains(x)).ToList();
            keysToRemove.ForEach(x => model.Remove(x));
            string updateValues = string.Join(", ", model.Where(x => x.Key != tableName + "ID")
                .Select(x => GetQueryValue(columns, x)));
            string query = $"UPDATE {tableName} SET {updateValues} WHERE {tableName}ID = {primaryKey}";
            await ExecuteSqlRaw(query);
        }

        private object GetQueryValue(List<TreeColumnViewModel> columns, KeyValuePair<string, object> model)
        {
            var columnDetail = columns.Find(x => x.ColumnName == model.Key);
            if (model.Value == null) return $"{model.Key} = null";
            switch (columnDetail.ColType)
            {
                case "int":
                    return $"{model.Key} = {Convert.ToInt32(model.Value)}";
                case "boolean":
                    return $"{model.Key} = {(Convert.ToBoolean(model.Value) ? 1 : 0)}";
                case "decimal":
                    return $"{model.Key} = {Convert.ToDecimal(model.Value)}";
                default:
                    return $"{model.Key} ='{model.Value.ToString().Replace("'", "''")}'";
            }
        }

        public async Task Destroy(string primaryKey, string tableName, int id)
        {
            string query = $"Delete from {tableName} WHERE {primaryKey} = {id}";
            await ExecuteSqlRaw(query);
        }

        public async Task<int> ExecuteSqlRaw(string query, IEnumerable<object> parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public async Task ExecuteSqlRaw(string query)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(query);
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
            string query = $"SELECT {type}ID,{optionCol} FROM {type}";
            var list = _dbContext.DynamicListFromSql(query, new Dictionary<string, object>(), "object")
                .Select(x => new DropdownOptions
                {
                    Id = x.Id,
                    Option = x.Option
                }).ToList();

            result = list.Select(x => new DropdownOptions
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
                var sqltblname = _dbContext.DynamicListFromSql($"SELECT FKTable  FROM MetaColumn WHERE TableName=@a and  DisplayName = @b", new Dictionary<string, object> { { "a", type }, { "b", optionCol } }).FirstOrDefault();
                var colName = _dbContext.DynamicListFromSql($"SELECT FKDisplayCol  FROM MetaColumn WHERE TableName=@a and  DisplayName = @b", new Dictionary<string, object> { { "a", type }, { "b", optionCol } }).FirstOrDefault();
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
                    PixelWidth = x.PixelWidth ?? 120,
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

        public DataTable PopulateSearchResult(DataTable searchTable, AdvanceSearchRequest request)
        {
            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.advancedSearch";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var uniqueIdCol = _dbContext.MetaColumn
                        .Where(x => x.TableName == request.TableName && x.Idcolumn == true)
                        .Select(x => x.ColumnName).FirstOrDefault() ?? "";

                    List<SqlParameter> parameters = new List<SqlParameter>()
                     {
                         new SqlParameter("@searchParams", SqlDbType.Structured) { Value = searchTable, TypeName= "dbo.SearchParams" },
                         new SqlParameter("@outputTable", SqlDbType.VarChar, 20) { Value = request.TableName },
                         new SqlParameter("@uniqueIDCol", SqlDbType.VarChar, 60) { Value = uniqueIdCol },
                         new SqlParameter("@countOnly", SqlDbType.Bit) { Value = request.CountOnly },
                         new SqlParameter("@offsetRows", SqlDbType.Int) { Value = request.PageNumber * request.PageSize },
                         new SqlParameter("@fetchRows", SqlDbType.Int) { Value = request.PageSize },
                         new SqlParameter("@sortByCol", SqlDbType.VarChar, 60) { Value = request.SortField ?? "" },
                         new SqlParameter("@sortByOrder", SqlDbType.VarChar, 10) { Value = request.SortDirection ?? "asc" },
                         new SqlParameter("@fromClause", SqlDbType.VarChar, 1000) { Value = request.FromClause }
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
                    FkdisplayCol = x.FkdisplayCol,
                    FkjoinCol = x.FkjoinCol,
                    Fktable = x.Fktable,
                    ColType = x.ColType,
                    IsEditable = x.IsEditable ?? false,
                    IsRequired = x.IsRequired ?? false,
                    ColSeq = x.ColSeq,
                    IsUnique = x.IsUnique ?? false,
                    IDColumn = x.Idcolumn ?? false,
                    PixelWidth = x.PixelWidth ?? 120,
                }).ToList();
        }

        public string GetFKDisplayColumn(string tableName, string displayName)
        {
            return _dbContext.DynamicListFromSql($"SELECT FKDisplayCol  FROM MetaColumn WHERE TableName=@a and  DisplayName = @b",
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