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
    public class WorkAuthorRepository : GenericRepository<WorkAuthor>, IWorkAuthorRepository
    {
        public WorkAuthorRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetWorkAuthors(DataSourceRequest request)
        {
            request.ApplyFilter();
            DataSourceResult list = await GetAll().Include(x=> x.Work).Include(x=> x.Author).ToDataSourceResultAsync(request);
            var data = (IEnumerable<WorkAuthor>)list.Data;
            var result = data.Select(x => new WorkAuthorViewModel
            {
                WorkAuthorID = x.WorkAuthorId,
                AuthorId = x.AuthorId,
                WorkId = x.WorkId,
                Role = x.Role,
                Work = new DropdownOptions
                {
                    Id = x.WorkId,
                    Option = x.Work?.Title
                },
                Author = new DropdownOptions
                {
                    Id = x.AuthorId,
                    Option = x.Author?.FullName
                }
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateWorkAuthor(WorkAuthorViewModel model)
        {
            var newWorkAuthor = new WorkAuthor
            {
                AuthorId = model.Author?.Id,
                WorkId = model.Work?.Id,
                Role = model.Role
            };
            await Create(newWorkAuthor);
            return newWorkAuthor.WorkAuthorId;
        }
        public async Task UpdateWorkAuthor(WorkAuthorViewModel model)
        {
            var dbWorkAuthor = await GetAll().Where(x => x.WorkAuthorId == model.WorkAuthorID).FirstOrDefaultAsync();
            if (dbWorkAuthor != null)
            {
                dbWorkAuthor.WorkId = model.Work?.Id;
                dbWorkAuthor.AuthorId = model.Author?.Id;
                dbWorkAuthor.Role = model.Role;
                await Update(dbWorkAuthor);
            }
        }
    }
}
