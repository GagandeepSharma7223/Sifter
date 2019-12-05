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
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetAuthors(DataSourceRequest request)
        {
            DataSourceResult list = new DataSourceResult();
            var query = GetAll();
            var stringCompareFilters = await ModifyFilters(new Author(), request.Filters, "Author");
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

            query = query.Include(x => x.BirthCountry);

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
            var authors = (IEnumerable<Author>)list.Data;
            var result = authors.Select(x => new AuthorViewModel
            {
                AuthorID = x.AuthorId,
                AlsoKnownAs = x.AlsoKnownAs,
                BirthCountryID = x.BirthCountryId,
                BirthYear = x.BirthYear,
                Comments = x.Comments,
                DeathYear = x.DeathYear,
                Occupation = x.Occupation,
                FirstActivityYear = x.FirstActivityYear,
                Gender = x.Gender,
                IsOrganization = x.IsOrganization,
                FullName = x.FullName,
                FirstName = x.FirstName,
                PenName = x.PenName,
                Sources = x.Sources,
                LastName = x.LastName,
                Title = x.Title,
                BirthCountry = new DropdownOptions
                {
                    Id = x.BirthCountry?.CountryId,
                    Option = x.BirthCountry?.Name
                }
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreateAuthor(AuthorViewModel model, bool updateForm = false)
        {
            var newAuthor = new Author
            {
                AlsoKnownAs = model.AlsoKnownAs,
                BirthCountryId = updateForm ? model.BirthCountryID : model.BirthCountry?.Id,
                BirthYear = model.BirthYear,
                Comments = model.Comments,
                DeathYear = model.DeathYear,
                Occupation = model.Occupation,
                Title = model.Title,
                FirstActivityYear = model.FirstActivityYear,
                Gender = model.Gender,
                IsOrganization = model.IsOrganization,
                FirstName= model.FirstName,
                FullName = model.FullName,
                PenName = model.PenName,
                Sources = model.Sources,
                LastName = model.LastName
            };
            await Create(newAuthor);
            return newAuthor.AuthorId;
        }
        public async Task UpdateAuthor(AuthorViewModel model, bool updateForm = false)
        {
            var dbAuthor = await GetAll().Where(x => x.AuthorId == model.AuthorID).FirstOrDefaultAsync();
            if (dbAuthor != null)
            {
                dbAuthor.AlsoKnownAs = model.AlsoKnownAs;
                dbAuthor.BirthCountryId = updateForm ? model.BirthCountryID : model.BirthCountry?.Id;
                dbAuthor.BirthYear = model.BirthYear;
                dbAuthor.Comments = model.Comments;
                dbAuthor.DeathYear = model.DeathYear;
                dbAuthor.Occupation = model.Occupation;
                dbAuthor.Title = model.Title;
                dbAuthor.FirstActivityYear = model.FirstActivityYear;
                dbAuthor.Gender = model.Gender;
                dbAuthor.IsOrganization = model.IsOrganization;
                dbAuthor.FirstName = model.FirstName;
                dbAuthor.FullName = model.FullName;
                dbAuthor.PenName = model.PenName;
                dbAuthor.Sources = model.Sources;
                dbAuthor.LastName = model.LastName;
                await Update(dbAuthor);
            }
        }
    }
}
