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
            request.ApplyFilter();
            DataSourceResult list = await GetAll().Include(x=> x.BirthCountry).ToDataSourceResultAsync(request);
            var authors = (IEnumerable<Author>)list.Data;
            var result = authors.Select(x => new AuthorViewModel
            {
                AuthorID = x.AuthorId,
                AlsoKnownAs = x.AlsoKnownAs,
                BirthCountryId = x.BirthCountryId,
                BirthYear = x.BirthYear,
                Comments = x.Comments,
                DeathYear = x.DeathYear,
                Occupation = x.Occupation,
                FirstActivityYear = x.FirstActivityYear,
                Gender = new TextDropdownOptions
                {
                    Option = x.Gender,
                    Id = x.Gender
                },
                IsOrganization = x.IsOrganization == "Yes" ? true: false,
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

        public async Task<int> CreateAuthor(AuthorViewModel model)
        {
            var newAuthor = new Author
            {
                AlsoKnownAs = model.AlsoKnownAs,
                BirthCountryId = model.BirthCountry?.Id,
                BirthYear = model.BirthYear,
                Comments = model.Comments,
                DeathYear = model.DeathYear,
                Occupation = model.Occupation,
                Title = model.Title,
                FirstActivityYear = model.FirstActivityYear,
                Gender = model.Gender?.Id,
                IsOrganization = model.IsOrganization ? "Yes" : "No",
                FirstName= model.FirstName,
                FullName = model.FullName,
                PenName = model.PenName,
                Sources = model.Sources,
                LastName = model.LastName
            };
            await Create(newAuthor);
            return newAuthor.AuthorId;
        }
        public async Task UpdateAuthor(AuthorViewModel model)
        {
            var dbAuthor = await GetAll().Where(x => x.AuthorId == model.AuthorID).FirstOrDefaultAsync();
            if (dbAuthor != null)
            {
                dbAuthor.AlsoKnownAs = model.AlsoKnownAs;
                dbAuthor.BirthCountryId = model.BirthCountry?.Id;
                dbAuthor.BirthYear = model.BirthYear;
                dbAuthor.Comments = model.Comments;
                dbAuthor.DeathYear = model.DeathYear;
                dbAuthor.Occupation = model.Occupation;
                dbAuthor.Title = model.Title;
                dbAuthor.FirstActivityYear = model.FirstActivityYear;
                dbAuthor.Gender = model.Gender?.Id;
                dbAuthor.IsOrganization = model.IsOrganization ? "Yes" : "No";
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
