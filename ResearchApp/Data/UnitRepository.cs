using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public class UnitRepository : GenericRepository<Unit>, IUnitRepository
    {
        public UnitRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetUnits(DataSourceRequest request)
        {
            try
            {
                DataSourceResult list = new DataSourceResult();
                var query = GetAll();
                var stringCompareFilters = await ModifyFilters(request.Filters, "Unit");
                request.ApplyFilter();

                if (stringCompareFilters.Any())
                {
                    foreach (var filter in stringCompareFilters)
                    {
                        if (!string.IsNullOrEmpty(filter.FKColumn))
                        {
                            query = query.OrderBy($"{filter.Entity}.{filter.FKColumn} asc");
                        }
                    }
                }

                query = query.Include(x => x.Category).Include(x => x.Work);

                if (stringCompareFilters.Any())
                {
                    string whereCondition = "";
                    List<object> whereConditionParams = new List<object>();
                    for (int i = 0; i < stringCompareFilters.Count; i++)
                    {
                        if (whereCondition != "")
                        {
                            whereCondition += " and ";
                        }
                        var stringFilter = stringCompareFilters[i];
                        whereCondition += $"{stringFilter.Entity} != @{whereConditionParams.Count} and ";
                        whereConditionParams.Add(null);
                        whereCondition += $"{stringFilter.Entity}.{stringFilter.FKColumn}.CompareTo(@{whereConditionParams.Count}) {GetOperatorSymbol(stringFilter.Operator)} 0";
                        whereConditionParams.Add(stringFilter.Value);
                    }
                    query = query.Where(whereCondition, whereConditionParams.ToArray());
                }

                list = await query.ToDataSourceResultAsync(request);

                var data = (IEnumerable<Unit>)list.Data;
                var result = data.Select(x => new UnitViewModel
                {
                    UnitID = x.UnitId,
                    CategoryId = x.CategoryId,
                    StartPage = x.StartPage,
                    Text = x.Text,
                    Title = x.Title,
                    LiteralTitle = x.LiteralTitle,
                    TitleEnglish = x.TitleEnglish,
                    WorkId = x.WorkId,
                    Work = new DropdownOptions
                    {
                        Id = x.Work?.WorkId,
                        Option = x.Work?.Title
                    },
                    Category = new DropdownOptions
                    {
                        Id = x.CategoryId,
                        Option = x.Category?.Name
                    }
                }).AsEnumerable();
                list.Data = result;
                return list;
            }
            catch (Exception ex)
            {
                //throw;
            }
            return new DataSourceResult();
        }

        public async Task<int> CreateUnit(UnitViewModel model)
        {
            var newUnit = new Unit
            {
                UnitId = model.UnitID,
                CategoryId = model.Category?.Id,
                StartPage = model.StartPage,
                Text = model.Text,
                Title = model.Title,
                LiteralTitle = model.LiteralTitle,
                TitleEnglish = model.TitleEnglish,
                WorkId = model.Work?.Id
            };
            await Create(newUnit);
            return newUnit.UnitId;
        }
        public async Task UpdateUnit(UnitViewModel model)
        {
            var dbUnit = await GetAll().Where(x => x.UnitId == model.UnitID).FirstOrDefaultAsync();
            if (dbUnit != null)
            {
                dbUnit.CategoryId = model.Category?.Id;
                dbUnit.StartPage = model.StartPage;
                dbUnit.Text = model.Text;
                dbUnit.Title = model.Title;
                dbUnit.LiteralTitle = model.LiteralTitle;
                dbUnit.TitleEnglish = model.TitleEnglish;
                dbUnit.WorkId = model.Work.Id;
                await Update(dbUnit);
            }
        }
    }
}
