using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetCountries(DataSourceRequest request)
        {
            DataSourceResult list = await GetAll().ToDataSourceResultAsync(request);
            var data = (IEnumerable<Country>)list.Data;
            var result = data.Select(x => new CountryViewModel
            {
                CountryID = x.CountryID,
                Name = x.Name,
                AlternateNames = x.AlternateNames,
                NameAscii = x.NameAscii,
                Code2 = x.Code2,
                Code3 = x.Code3,
                Continent = x.Continent,
                GeoNameID = x.GeoNameID,
                Phone = x.Phone,
                Slug = x.Slug,
                TLD = x.TlD
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateCountry(CountryViewModel model, bool updateForm = false)
        {
            var newCountry = new Country
            {
                Name = model.Name,
                AlternateNames = model.AlternateNames,
                NameAscii = model.NameAscii,
                Code2 = model.Code2,
                Code3 = model.Code3,
                Continent = model.Continent,
                GeoNameID = model.GeoNameID,
                Phone = model.Phone,
                Slug = model.Slug,
                TlD = model.TLD
            };
            await Create(newCountry);
            return newCountry.CountryID;
        }
        public async Task UpdateCountry(CountryViewModel model, bool updateForm = false)
        {
            var dbCountry = await GetAll().Where(x => x.CountryID == model.CountryID).FirstOrDefaultAsync();
            if (dbCountry != null)
            {
                dbCountry.Name = model.Name;
                dbCountry.AlternateNames = model.AlternateNames;
                dbCountry.NameAscii = model.NameAscii;
                dbCountry.Code2 = model.Code2;
                dbCountry.Code3 = model.Code3;
                dbCountry.Continent = model.Continent;
                dbCountry.GeoNameID = model.GeoNameID;
                dbCountry.Phone = model.Phone;
                dbCountry.Slug = model.Slug;
                dbCountry.TlD = model.TLD;
                await Update(dbCountry);
            }
        }
    }
}
