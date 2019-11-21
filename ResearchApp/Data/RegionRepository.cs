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
    public class RegionRepository : GenericRepository<Region>, IRegionRepository
    {
        public RegionRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetRegions(DataSourceRequest request)
        {
            DataSourceResult list = new DataSourceResult();
            var query = GetAll();
            var stringCompareFilters = await ModifyFilters(request.Filters, "Region");
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

            query = query.Include(x => x.Country);

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
            var data = (IEnumerable<Region>)list.Data;
            var result = data.Select(x => new RegionViewModel
            {
                RegionID = x.RegionId,
                Name = x.Name,
                AlternateNames = x.AlternateNames,
                CountryId = x.CountryId,
                NameAscii = x.NameAscii,
                DisplayName = x.DisplayName,
                GeoNameCode = x.GeoNameCode,
                GeoNameId = x.GeoNameId,
                Slug = x.Slug,
                Country = new DropdownOptions
                {
                    Id = x.CountryId,
                    Option = x.Country?.Name
                }
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateRegion(RegionViewModel model)
        {
            var newRegion = new Region
            {
                Name = model.Name,
                AlternateNames = model.AlternateNames,
                CountryId = model.Country?.Id,
                NameAscii = model.NameAscii,
                DisplayName = model.DisplayName,
                GeoNameCode = model.GeoNameCode,
                GeoNameId = model.GeoNameId,
                Slug = model.Slug
            };
            await Create(newRegion);
            return newRegion.RegionId;
        }
        public async Task UpdateRegion(RegionViewModel model)
        {
            var dbRegion = await GetAll().Where(x => x.RegionId == model.RegionID).FirstOrDefaultAsync();
            if (dbRegion != null)
            {
                dbRegion.Name = model.Name;
                dbRegion.AlternateNames = model.AlternateNames;
                dbRegion.CountryId = model.Country?.Id;
                dbRegion.NameAscii = model.NameAscii;
                dbRegion.DisplayName = model.DisplayName;
                dbRegion.GeoNameCode = model.GeoNameCode;
                dbRegion.GeoNameId = model.GeoNameId;
                dbRegion.Slug = model.Slug;
                await Update(dbRegion);
            }
        }
    }
}
