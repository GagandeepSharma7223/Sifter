using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
            request.ApplyFilter();
            DataSourceResult list = await GetAll().Include(x=> x.Country).ToDataSourceResultAsync(request);
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
            //int newRegionId = await GetAll().MaxAsync(x => x.RegionId);
            //newRegionId++;
            var newRegion = new Region
            {
                //RegionId = newRegionId,
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
                //dbRegion.RegionId = model.RegionId;
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
