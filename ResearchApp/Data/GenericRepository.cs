//using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ResearchApp.Data;

namespace ResearchApp.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class
    {
        public readonly SifterContext _dbContext;

        public GenericRepository(SifterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public async Task Create(TEntity entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
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

        public List<dynamic> GetFilterData(string type, string optionCol, int page, int pageSize)
        {
            return _dbContext.DynamicListFromSql($"SELECT DISTINCT {optionCol} FROM {type} WHERE {optionCol}!=@a ORDER BY {optionCol} OFFSET { (page - 1) * pageSize } ROWS FETCH NEXT { pageSize } ROWS ONLY", new Dictionary<string, object> { { "a", string.Empty } }).ToList();
            //return _dbContext.Query($"ResearchApp.Models.{type}")
            //return _dbContext.Query($"ResearchApp.Models.{type}")
            //    .Select($"new ({optionCol})").Distinct();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
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