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
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetLanguages(DataSourceRequest request, bool filterRequest = false)
        {
            DataSourceResult list = await GetAll().ToDataSourceResultAsync(request);
            var data = (IEnumerable<Language>)list.Data;
            if (filterRequest)
            {
                data = data.OrderBy(x => x.LanguageID);
            }
            var result = data.Select(x => new LanguageViewModel
            {
                LanguageID = x.LanguageID,
                Name = x.Name
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateLanguage(LanguageViewModel model, bool updateForm = false)
        {
            var newLanguage = new Language
            {
                Name = model.Name
            };
            await Create(newLanguage);
            return newLanguage.LanguageID;
        }
        public async Task UpdateLanguage(LanguageViewModel model, bool updateForm = false)
        {
            var dbLanguage = await GetAll().Where(x => x.LanguageID == model.LanguageID).FirstOrDefaultAsync();
            if (dbLanguage != null)
            {
                dbLanguage.Name = model.Name;
                await Update(dbLanguage);
            }
        }
    }
}
