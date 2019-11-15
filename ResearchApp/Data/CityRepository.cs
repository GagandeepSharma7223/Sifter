using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(SifterContext dbContext)
        : base(dbContext)
        {

        }

        public async Task<DataSourceResult> GetCities(DataSourceRequest request)
        {
            request.ApplyFilter();
            DataSourceResult list = await GetAll().Include(x => x.Country).Include(x => x.Region).ToDataSourceResultAsync(request);
            var data = (IEnumerable<City>)list.Data;
            var result = data.Select(x => new CityViewModel
            {
                CityID = x.CityId,
                Name = x.Name,
                AlternateNames = x.AlternateNames,
                CountryId = x.CountryId,
                DisplayName = x.DisplayName,
                FeatureCode = x.FeatureCode,
                GeoNameId = x.GeoNameId,
                Latitude = x.Latitude,
                SearchNames = x.SearchNames,
                Longitude = x.Longitude,
                NameAscii = x.NameAscii,
                Population = x.Population,
                RegionId = x.RegionId,
                Slug = x.Slug,
                TimeZone = x.TimeZone,
                Country = new DropdownOptions
                {
                    Id = x.CountryId,
                    Option = x.Country?.Name
                },
                Region = new DropdownOptions
                {
                    Id = x.RegionId,
                    Option = x.Region?.Name
                }
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateCity(CityViewModel model)
        {
            var newCity = new City
            {
                Name = model.Name,
                AlternateNames = model.AlternateNames,
                CountryId = model.Country?.Id,
                DisplayName = model.DisplayName,
                FeatureCode = model.FeatureCode,
                GeoNameId = model.GeoNameId,
                Latitude = model.Latitude,
                SearchNames = model.SearchNames,
                Longitude = model.Longitude,
                NameAscii = model.NameAscii,
                Population = model.Population,
                RegionId = model.Region?.Id,
                Slug = model.Slug,
                TimeZone = model.TimeZone
            };
            await Create(newCity);
            return newCity.CityId;
        }
        public async Task UpdateCity(CityViewModel model)
        {
            var dbCity = await GetAll().Where(x => x.CityId == model.CityID).FirstOrDefaultAsync();
            if (dbCity != null)
            {
                dbCity.Name = model.Name;
                dbCity.AlternateNames = model.AlternateNames;
                dbCity.CountryId = model.Country?.Id;
                dbCity.DisplayName = model.DisplayName;
                dbCity.FeatureCode = model.FeatureCode;
                dbCity.GeoNameId = model.GeoNameId;
                dbCity.Latitude = model.Latitude;
                dbCity.SearchNames = model.SearchNames;
                dbCity.Longitude = model.Longitude;
                dbCity.NameAscii = model.NameAscii;
                dbCity.Population = model.Population;
                dbCity.RegionId = model.Region?.Id;
                dbCity.Slug = model.Slug;
                dbCity.TimeZone = model.TimeZone;
                await Update(dbCity);
            }
        }
    }
}
