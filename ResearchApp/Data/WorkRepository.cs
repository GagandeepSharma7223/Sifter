using Kendo.Mvc;
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
using System.Reflection;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public class WorkRepository : GenericRepository<Work>, IWorkRepository
    {
        public WorkRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetWorks(DataSourceRequest request, bool filterRequest = false)
        {
            DataSourceResult list = new DataSourceResult();
            try
            {
                var query = GetAll();
                var stringCompareFilters = await ModifyFilters(new Work(), request.Filters, "Work");
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
                query = query.Include(x => x.Author).Include(x => x.Translator).Include(x => x.Editor).Include(x => x.Publisher)
                    .Include(x => x.Language).Include(x => x.City);

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
                var books = (IEnumerable<Work>)list.Data;
                var result = books.Select(x => new WorkViewModel
                {
                    WorkID = x.WorkId,
                    PublicationYear = x.PublicationYear,
                    Title = x.Title,
                    TitleEnglish = x.TitleEnglish,
                    TitleLiteral = x.TitleLiteral,
                    AuthorID = x.AuthorId,
                    CityID = x.CityId,
                    LanguageID = x.LanguageId,
                    PublisherID = x.PublisherId,
                    TranslatorID = x.TranslatorId,
                    EditorID = x.EditorId,
                    Author = new DropdownOptions
                    {
                        Option = x.Author?.FullName,
                        Id = x.AuthorId
                    },
                    City = new DropdownOptions
                    {
                        Id = x.City?.CityId,
                        Option = x.City?.Name
                    },
                    Language = new DropdownOptions
                    {
                        Id = x.Language?.LanguageId,
                        Option = x.Language?.Name
                    },
                    Publisher = new DropdownOptions
                    {
                        Id = x.PublisherId,
                        Option = x.Publisher?.Name
                    },
                    Editor = new DropdownOptions
                    {
                        Option = x.Editor?.FullName,
                        Id = x.EditorId
                    },
                    Translator = new DropdownOptions
                    {
                        Option = x.Translator?.FullName,
                        Id = x.TranslatorId
                    }
                }).AsEnumerable();
                list.Data = result;
                return list;
            }
            catch (Exception e)
            {
                //throw;
            }
            return list;
        }

        public async Task<int> CreateWork(WorkViewModel model)
        {
            var newWork = new Work
            {
                PublicationYear = model.PublicationYear,
                Title = model.Title,
                TitleEnglish = model.TitleEnglish,
                TitleLiteral = model.TitleLiteral,
                AuthorId = model.Author?.Id,
                CityId = model.City?.Id,
                LanguageId = model.Language?.Id,
                PublisherId = model.Publisher?.Id,
                TranslatorId = model.Translator?.Id,
                EditorId = model.Editor?.Id
            };
            await Create(newWork);
            return newWork.WorkId;
        }
        public async Task UpdateWork(WorkViewModel model)
        {
            var dbWork = await GetAll().Where(x => x.WorkId == model.WorkID).FirstOrDefaultAsync();
            if (dbWork != null)
            {
                dbWork.PublicationYear = model.PublicationYear;
                dbWork.Title = model.Title;
                dbWork.TitleEnglish = model.TitleEnglish;
                dbWork.TitleLiteral = model.TitleLiteral;
                dbWork.AuthorId = model.Author == null ? model.AuthorID : model.Author.Id;
                dbWork.CityId = model.City == null ? model.CityID : model.City.Id;
                dbWork.LanguageId = model.Language == null ? model.LanguageID : model.Language.Id;
                dbWork.PublisherId = model.Publisher == null ? model.PublisherID : model.Publisher.Id;
                dbWork.TranslatorId = model.Translator == null ? model.TranslatorID : model.Translator.Id;
                dbWork.EditorId = model.Editor == null ? model.EditorID : model.Editor.Id;
                await Update(dbWork);
            }
        }
    }
}
