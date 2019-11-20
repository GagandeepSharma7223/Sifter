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
                GetFilters(request);
                request.ApplyFilter();
                list = await GetAll().OrderBy(x => x.WorkId).Include(x => x.Author).Include(x => x.Translator).Include(x => x.Editor).Include(x => x.Publisher)
                .Include(x => x.Language).Include(x => x.City)
                .ToDataSourceResultAsync(request);
                var books = (IEnumerable<Work>)list.Data;
                var result = books.Select(x => new WorkViewModel
                {
                    WorkID = x.WorkId,
                    PublicationYear = x.PublicationYear,
                    Title = x.Title,
                    TitleEnglish = x.TitleEnglish,
                    TitleLiteral = x.TitleLiteral,
                    AuthorId = x.AuthorId,
                    CityId = x.CityId,
                    LanguageId = x.LanguageId,
                    PublisherId = x.PublisherId,
                    TranslatorId = x.TranslatorId,
                    EditorId = x.EditorId,
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

        private static void GetFilters(DataSourceRequest request)
        {
            request.Filters.Each(item =>
            {
                var type = item.GetType();
                if (type == typeof(FilterDescriptor))
                {
                    var descriptor = (FilterDescriptor)item;
                    PropertyInfo propInfo = GetPropertyType(descriptor.Member);
                    if (propInfo.PropertyType == typeof(DropdownOptions))
                    {
                        descriptor.Member = descriptor.Member + "Id";
                    }
                }
                else if (type == typeof(CompositeFilterDescriptor))
                {
                    GetNestedFilter(item);
                }
            });
        }

        private static void GetNestedFilter(IFilterDescriptor item)
        {
            var filter = (CompositeFilterDescriptor)item;
            filter.FilterDescriptors.Each(desc =>
            {
                var type = desc.GetType();
                if (type == typeof(FilterDescriptor))
                {
                    var descriptor = (FilterDescriptor)desc;
                    PropertyInfo propInfo = GetPropertyType(descriptor.Member);
                    if (propInfo.PropertyType == typeof(DropdownOptions))
                    {
                        descriptor.Member = descriptor.Member + "Id";
                    }
                }
                else if (type == typeof(CompositeFilterDescriptor))
                {
                    GetNestedFilter(desc);
                }
            });
        }

        private static PropertyInfo GetPropertyType(string propName)
        {
            var obj = new WorkViewModel();
            Type type = obj.GetType();
            PropertyInfo propInfo = type.GetProperty(propName);
            return propInfo;
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
                dbWork.AuthorId = model.Author?.Id;
                dbWork.CityId = model.City?.Id;
                dbWork.LanguageId = model.Language?.Id;
                dbWork.PublisherId = model.Publisher?.Id;
                dbWork.TranslatorId = model.Translator?.Id;
                dbWork.EditorId = model.Editor?.Id;
                await Update(dbWork);
            }
        }
    }
}
