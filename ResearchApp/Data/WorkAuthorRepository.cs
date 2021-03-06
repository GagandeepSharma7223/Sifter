﻿using Kendo.Mvc.Extensions;
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
    public class WorkAuthorRepository : GenericRepository<WorkAuthor>, IWorkAuthorRepository
    {
        public WorkAuthorRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetWorkAuthors(DataSourceRequest request)
        {
            DataSourceResult list = new DataSourceResult();
            var query = GetAll();
            var stringCompareFilters = await ModifyFilters(new WorkAuthor(), request.Filters, "WorkAuthor");
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

            query = query.Include(x => x.Work).Include(x => x.Author).Include(x => x.Work);

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
            var data = (IEnumerable<WorkAuthor>)list.Data;
            var result = data.Select(x => new WorkAuthorViewModel
            {
                WorkAuthorID = x.WorkAuthorID,
                AuthorID = x.AuthorID,
                WorkID = x.WorkID,
                Role = x.Role,
                Work = new DropdownOptions
                {
                    Id = x.WorkID,
                    Option = x.Work?.Title
                },
                Author = new DropdownOptions
                {
                    Id = x.AuthorID,
                    Option = x.Author?.FullName
                }
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateWorkAuthor(WorkAuthorViewModel model, bool updateForm = false)
        {
            var newWorkAuthor = new WorkAuthor
            {
                AuthorID = updateForm ? model.AuthorID : model.Author?.Id,
                WorkID = updateForm ? model.WorkID : model.Work?.Id,
                Role = model.Role
            };
            await Create(newWorkAuthor);
            return newWorkAuthor.WorkAuthorID;
        }
        public async Task UpdateWorkAuthor(WorkAuthorViewModel model, bool updateForm = false)
        {
            var dbWorkAuthor = await GetAll().Where(x => x.WorkAuthorID == model.WorkAuthorID).FirstOrDefaultAsync();
            if (dbWorkAuthor != null)
            {
                dbWorkAuthor.WorkID = updateForm ? model.WorkID : model.Work?.Id;
                dbWorkAuthor.AuthorID = updateForm ? model.AuthorID : model.Author?.Id;
                dbWorkAuthor.Role = model.Role;
                await Update(dbWorkAuthor);
            }
        }
    }
}
