using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetCities(DataSourceRequest request)
        {
            DataSourceResult list = new DataSourceResult();
            var query = GetAll();
            var stringCompareFilters = await ModifyFilters(new City(), request.Filters, "City");
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
            query = query.Include(x => x.Country).Include(x => x.Region);

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
            var data = (IEnumerable<City>)list.Data;
            var result = data.Select(x => new CityViewModel
            {
                CityID = x.CityID,
                Name = x.Name,
                AlternateNames = x.AlternateNames,
                CountryID = x.CountryID,
                DisplayName = x.DisplayName,
                FeatureCode = x.FeatureCode,
                GeoNameID = x.GeoNameID,
                Latitude = x.Latitude,
                SearchNames = x.SearchNames,
                Longitude = x.Longitude,
                NameAscii = x.NameAscii,
                Population = x.Population,
                RegionID = x.RegionID,
                Slug = x.Slug,
                TimeZone = x.TimeZone,
                Country = new DropdownOptions
                {
                    Id = x.CountryID,
                    Option = x.Country?.Name
                },
                Region = new DropdownOptions
                {
                    Id = x.RegionID,
                    Option = x.Region?.Name
                }
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateCity(CityViewModel model, bool updateForm = false)
        {
            var newCity = new City
            {
                Name = model.Name,
                AlternateNames = model.AlternateNames,
                CountryID = updateForm ? model.CountryID : model.Country?.Id,
                DisplayName = model.DisplayName,
                FeatureCode = model.FeatureCode,
                GeoNameID = model.GeoNameID,
                Latitude = model.Latitude,
                SearchNames = model.SearchNames,
                Longitude = model.Longitude,
                NameAscii = model.NameAscii,
                Population = model.Population,
                RegionID = updateForm ? model.RegionID : model.Region?.Id,
                Slug = model.Slug,
                TimeZone = model.TimeZone
            };
            await Create(newCity);
            return newCity.CityID;
        }
        public async Task UpdateCity(CityViewModel model, bool updateForm = false)
        {
            var dbCity = await GetAll().Where(x => x.CityID == model.CityID).FirstOrDefaultAsync();
            if (dbCity != null)
            {
                dbCity.Name = model.Name;
                dbCity.AlternateNames = model.AlternateNames;
                dbCity.CountryID = updateForm ? model.CountryID : model.Country?.Id;
                dbCity.DisplayName = model.DisplayName;
                dbCity.FeatureCode = model.FeatureCode;
                dbCity.GeoNameID = model.GeoNameID;
                dbCity.Latitude = model.Latitude;
                dbCity.SearchNames = model.SearchNames;
                dbCity.Longitude = model.Longitude;
                dbCity.NameAscii = model.NameAscii;
                dbCity.Population = model.Population;
                dbCity.RegionID = updateForm ? model.RegionID : model.Region?.Id;
                dbCity.Slug = model.Slug;
                dbCity.TimeZone = model.TimeZone;
                await Update(dbCity);
            }
        }
    }
}
