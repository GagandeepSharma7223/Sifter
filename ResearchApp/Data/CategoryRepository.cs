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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetCategories(DataSourceRequest request)
        {
            DataSourceResult list = await GetAll().ToDataSourceResultAsync(request);
            var data = (IEnumerable<Category>)list.Data;
            var result = data.Select(x => new CategoryViewModel
            {
                CategoryID = x.CategoryId,
                Name = x.Name
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateCategory(CategoryViewModel model, bool updateForm = false)
        {
            var newCategory = new Category
            {
                Name = model.Name
            };
            await Create(newCategory);
            return newCategory.CategoryId;
        }
        public async Task UpdateCategory(CategoryViewModel model, bool updateForm = false)
        {
            var dbCategory = await GetAll().Where(x => x.CategoryId == model.CategoryID).FirstOrDefaultAsync();
            if (dbCategory != null)
            {
                dbCategory.Name = model.Name;
                await Update(dbCategory);
            }
        }
    }
}
