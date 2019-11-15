using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ResearchApp.Data;
//using ResearchApp.Data.Extensions;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ResearchApp.Controllers
{
    public class GridController : Controller
    {
        private readonly IWorkRepository _workRepo;
        private readonly IAuthorRepository _authorRepo;
        private readonly IPublisherRepository _publisherRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILanguageRepository _languageRepo;
        private readonly ICityRepository _cityRepo;
        private readonly IRegionRepository _regionRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly IWorkAuthorRepository _workAuthorRepo;
        private readonly IUnitRepository _unitRepo;
        private IMemoryCache _cache;

        MemoryCacheEntryOptions CacheEntryOptions = new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromHours(1));

        public GridController(IMemoryCache memoryCache, IWorkRepository workRepo, IAuthorRepository authorRepo, IPublisherRepository publisherRepo,
            ICategoryRepository categoryRepo, ILanguageRepository languageRepo, ICityRepository cityRepo, IRegionRepository regionRepo,
            ICountryRepository countryRepo, IWorkAuthorRepository workAuthorRepo, IUnitRepository unitRepo)
        {
            _workRepo = workRepo;
            _publisherRepo = publisherRepo;
            _authorRepo = authorRepo;
            _categoryRepo = categoryRepo;
            _languageRepo = languageRepo;
            _cityRepo = cityRepo;
            _regionRepo = regionRepo;
            _countryRepo = countryRepo;
            _workAuthorRepo = workAuthorRepo;
            _unitRepo = unitRepo;
            _cache = memoryCache;
        }

        #region Books
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Works_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkViewModel> works)
        {
            var results = new List<WorkViewModel>();

            if (works != null && ModelState.IsValid)
            {
                foreach (var work in works)
                {
                    work.WorkID = await _workRepo.CreateWork(work);
                    results.Add(work);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Works_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkViewModel> works)
        {
            if (works != null && ModelState.IsValid)
            {
                foreach (var work in works)
                {
                    await _workRepo.UpdateWork(work);
                }
            }
            return Json(works.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Works_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkViewModel> works)
        {
            if (works.Any())
            {
                foreach (var work in works)
                {
                    await _workRepo.Delete(work.WorkID.GetValueOrDefault());
                }
            }
            return Json(works.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Authors
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Authors_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AuthorViewModel> authors)
        {
            var results = new List<AuthorViewModel>();

            if (authors != null && ModelState.IsValid)
            {
                foreach (var author in authors)
                {
                    author.AuthorID = await _authorRepo.CreateAuthor(author);
                    results.Add(author);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Authors_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AuthorViewModel> authors)
        {
            if (authors != null && ModelState.IsValid)
            {
                foreach (var author in authors)
                {
                    await _authorRepo.UpdateAuthor(author);
                }
            }
            return Json(authors.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Authors_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AuthorViewModel> authors)
        {
            if (authors.Any())
            {
                foreach (var author in authors)
                {
                    await _authorRepo.Delete(author.AuthorID);

                }
            }
            return Json(authors.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Publishers
        public IActionResult Publishers()
        {
            return PartialView("~/Views/Home/_PartialPublisherList.cshtml");
        }

        public async Task<IActionResult> Publishers_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult list = await _publisherRepo.GetPublishers(request);
            return Json(list);
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Publishers_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PublisherViewModel> publishers)
        {
            var results = new List<PublisherViewModel>();

            if (publishers != null && ModelState.IsValid)
            {
                foreach (var publisher in publishers)
                {
                    publisher.PublisherID = await _publisherRepo.CreatePublisher(publisher);
                    results.Add(publisher);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Publishers_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PublisherViewModel> publishers)
        {
            if (publishers != null && ModelState.IsValid)
            {
                foreach (var publisher in publishers)
                {
                    await _publisherRepo.UpdatePublisher(publisher);
                }
            }
            return Json(publishers.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Publishers_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PublisherViewModel> publishers)
        {
            if (publishers.Any())
            {
                foreach (var publisher in publishers)
                {
                    await _publisherRepo.Delete(publisher.PublisherID.GetValueOrDefault());
                }
            }
            return Json(publishers.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Common Methods
        public IActionResult GetView(GridTypes type)
        {
            string view = "~/Views/Home/_PartialBookList.cshtml";
            switch (type)
            {
                case GridTypes.Category:
                    view = "~/Views/Home/_PartialCategoryList.cshtml";
                    break;
                case GridTypes.Language:
                    view = "~/Views/Home/_PartialLanguageList.cshtml";
                    break;
                case GridTypes.City:
                    view = "~/Views/Home/_PartialCityList.cshtml";
                    break;
                case GridTypes.Region:
                    view = "~/Views/Home/_PartialRegionList.cshtml";
                    break;
                case GridTypes.Country:
                    view = "~/Views/Home/_PartialCountryList.cshtml";
                    break;
                case GridTypes.Publisher:
                    view = "~/Views/Home/_PartialPublisherList.cshtml";
                    break;
                case GridTypes.Work:
                    view = "~/Views/Home/_PartialBookList.cshtml";
                    break;
                case GridTypes.Author:
                    PopulateGenders();
                    PopulateYesNo();
                    view = "~/Views/Home/_PartialAuthorList.cshtml";
                    break;
                case GridTypes.WorkAuthor:
                    view = "~/Views/Home/_PartialWorkAuthorList.cshtml";
                    break;
                case GridTypes.Unit:
                    view = "~/Views/Home/_PartialUnitList.cshtml";
                    break;
            }
            return PartialView(view);
        }

        public async Task<IActionResult> List([DataSourceRequest]DataSourceRequest request, GridTypes type)
        {
            DataSourceResult list = new DataSourceResult();

            // Filter Data
            if (request.Page > 0 && request.PageSize == 0)
            {
                request.PageSize = 1000;
                if (!_cache.TryGetValue(type.ToString(), out DataSourceResult cacheData))
                {
                    cacheData = await PopulateGrid(request, type, true);
                    _cache.Set(type.ToString(), cacheData, CacheEntryOptions);
                }
                list = cacheData;
                return Json(list);
            }
            list = await PopulateGrid(request, type);
            return Json(list);
        }

        public async Task<IActionResult> FilterList([DataSourceRequest]DataSourceRequest request, GridTypes type)
        {
            DataSourceResult list = new DataSourceResult();

            // Filter Data
            if (request.Page > 0 && request.PageSize == 0)
            {
                request.PageSize = 1000;
                if (!_cache.TryGetValue(type.ToString(), out DataSourceResult cacheData))
                {
                    cacheData = await PopulateGrid(request, type, true);
                    _cache.Set(type.ToString(), cacheData, CacheEntryOptions);
                }
                list = cacheData;
                return Json(list);
            }
            list = await PopulateGrid(request, type);
            return Json(list);
        }

        private async Task<DataSourceResult> PopulateGrid(DataSourceRequest request, GridTypes type, bool filterRequest = false)
        {
            DataSourceResult list = new DataSourceResult();
            switch (type)
            {
                case GridTypes.Language:
                    list = await _languageRepo.GetLanguages(request);
                    break;
                case GridTypes.City:
                    list = await _cityRepo.GetCities(request);
                    break;
                case GridTypes.Region:
                    list = await _regionRepo.GetRegions(request);
                    break;
                case GridTypes.Country:
                    list = await _countryRepo.GetCountries(request);
                    break;
                case GridTypes.Publisher:
                    list = await _publisherRepo.GetPublishers(request);
                    break;
                case GridTypes.Work:
                    list = await _workRepo.GetWorks(request);
                    break;
                case GridTypes.Author:
                    list = await _authorRepo.GetAuthors(request);
                    break;
                case GridTypes.WorkAuthor:
                    list = await _workAuthorRepo.GetWorkAuthors(request);
                    break;
                case GridTypes.Unit:
                    list = await _unitRepo.GetUnits(request);
                    break;
                case GridTypes.Category:
                    list = await _categoryRepo.GetCategories(request);
                    break;
            }

            return list;
        }

        //[AcceptVerbs("Get")]
        public IActionResult BindFilterDropDown([DataSourceRequest] DataSourceRequest request, string treeTable, string optionCol, string fieldType)
        {
            if(request.PageSize == 0)
            {

                request.PageSize = 10000;
            }
            var result = _workRepo.GetFilterData(treeTable, optionCol, request.Page, request.PageSize, fieldType);
            return Json(result);
        }

        [AcceptVerbs("Post")]
        public IActionResult BindFilterListView([DataSourceRequest] DataSourceRequest request, string treeTable, string optionCol, string fieldType)
        {
            var result = _workRepo.GetFilterData(treeTable, optionCol, request.Page, request.PageSize, fieldType);
            return Json(result);
        }


        [AcceptVerbs("Post")]
        [ResponseCache(Duration = 3600)]
        public IActionResult GetDropdownOptions([DataSourceRequest] DataSourceRequest request, string treeTable, string optionCol)
        {
            if (!_cache.TryGetValue($"{treeTable}Options", out List<DropdownOptions> cachedDetails))
            {
                cachedDetails = _workRepo.GetOptions(treeTable, optionCol);
                _cache.Set($"{treeTable}Options", cachedDetails, CacheEntryOptions);
            }
            return Json(cachedDetails.ToDataSourceResult(request));
        }

        public IActionResult PopulateDD(string treeTable, string optionCol)
        {
            if (!_cache.TryGetValue($"{treeTable}Options", out List<DropdownOptions> cachedDetails))
            {
                cachedDetails = _workRepo.GetOptions(treeTable, optionCol);
                _cache.Set($"{treeTable}Options", cachedDetails, CacheEntryOptions);
            }
            return Json(cachedDetails);
        }

        //public IActionResult PopulateGrid(string treeTable, string optionCol)
        //{
        //    if (!_cache.TryGetValue($"{treeTable}Options", out List<DropdownOptions> cachedDetails))
        //    {
        //        cachedDetails = _workRepo.GetOptions(treeTable, optionCol);
        //        var cacheEntryOptions = new MemoryCacheEntryOptions()
        //           .SetSlidingExpiration(TimeSpan.FromHours(1));

        //        _cache.Set($"{treeTable}Options", cachedDetails, cacheEntryOptions);
        //    }
        //    return Json(cachedDetails);
        //}

        public IActionResult Dropdown_ValueMapper(int[] values, string treeTable = "Author")
        {
            var indices = new List<int>();

            if (values != null && values.Any())
            {
                var index = 0;
                if (_cache.TryGetValue($"{treeTable}Options", out List<DropdownOptions> cachedDetails))
                {
                    foreach (var item in cachedDetails)
                    {
                        if (values.Contains(item.Id.GetValueOrDefault()))
                        {
                            indices.Add(index);
                        }
                        index += 1;
                    }
                }
            }

            return Json(indices);
        }

        private void PopulateGenders()
        {
            var genders = new List<TextDropdownOptions>
            {
                new TextDropdownOptions{
                    Id = "",
                    Option = ""
                },
                new TextDropdownOptions{
                    Id = "Male",
                    Option = "Male"
                },
                new TextDropdownOptions{
                    Id = "Female",
                    Option = "Female"
                }
            };
            ViewData["genders"] = genders;
        }

        private void PopulateYesNo()
        {
            var genders = new List<TextDropdownOptions>
            {
                new TextDropdownOptions{
                    Id = "",
                    Option = ""
                },
                new TextDropdownOptions{
                    Id = "No",
                    Option = "No"
                },
                new TextDropdownOptions{
                    Id = "Yes",
                    Option = "Yes"
                }
            };
            ViewData["yesNoOptions"] = genders;
        }
        #endregion

        #region Category
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Category_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CategoryViewModel> list)
        {
            var results = new List<CategoryViewModel>();

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    item.CategoryID = await _categoryRepo.CreateCategory(item);
                    results.Add(item);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Category_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CategoryViewModel> list)
        {
            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    await _categoryRepo.UpdateCategory(item);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Category_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CategoryViewModel> list)
        {
            if (list.Any())
            {
                foreach (var item in list)
                {
                    await _publisherRepo.Delete(item.CategoryID.GetValueOrDefault());
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Language
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Language_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<LanguageViewModel> list)
        {
            var results = new List<LanguageViewModel>();

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    item.LanguageID = await _languageRepo.CreateLanguage(item);
                    results.Add(item);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Language_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<LanguageViewModel> list)
        {
            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    await _languageRepo.UpdateLanguage(item);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Language_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<LanguageViewModel> list)
        {
            if (list.Any())
            {
                foreach (var item in list)
                {
                    await _publisherRepo.Delete(item.LanguageID.GetValueOrDefault());
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region City
        [AcceptVerbs("Post")]
        public async Task<IActionResult> City_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CityViewModel> list)
        {
            var results = new List<CityViewModel>();

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    item.CityID = await _cityRepo.CreateCity(item);
                    results.Add(item);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> City_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CityViewModel> list)
        {
            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    await _cityRepo.UpdateCity(item);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> City_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CityViewModel> list)
        {
            if (list.Any())
            {
                foreach (var item in list)
                {
                    await _publisherRepo.Delete(item.CityID.GetValueOrDefault());
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Region
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Region_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<RegionViewModel> list)
        {
            var results = new List<RegionViewModel>();

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    item.RegionID = await _regionRepo.CreateRegion(item);
                    results.Add(item);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Region_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<RegionViewModel> list)
        {
            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    await _regionRepo.UpdateRegion(item);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Region_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<RegionViewModel> list)
        {
            if (list.Any())
            {
                foreach (var item in list)
                {
                    await _publisherRepo.Delete(item.RegionID.GetValueOrDefault());
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Country
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Country_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CountryViewModel> list)
        {
            var results = new List<CountryViewModel>();

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    item.CountryID = await _countryRepo.CreateCountry(item);
                    results.Add(item);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Country_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CountryViewModel> list)
        {
            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    await _countryRepo.UpdateCountry(item);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Country_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CountryViewModel> list)
        {
            if (list.Any())
            {
                foreach (var item in list)
                {
                    await _publisherRepo.Delete(item.CountryID.GetValueOrDefault());
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region WorkAuthor
        [AcceptVerbs("Post")]
        public async Task<IActionResult> WorkAuthor_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkAuthorViewModel> list)
        {
            var results = new List<WorkAuthorViewModel>();

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    item.WorkAuthorID = await _workAuthorRepo.CreateWorkAuthor(item);
                    results.Add(item);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> WorkAuthor_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkAuthorViewModel> list)
        {
            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    await _workAuthorRepo.UpdateWorkAuthor(item);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> WorkAuthor_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkAuthorViewModel> list)
        {
            if (list.Any())
            {
                foreach (var item in list)
                {
                    await _publisherRepo.Delete(item.WorkAuthorID);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Unit
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Unit_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UnitViewModel> list)
        {
            var results = new List<UnitViewModel>();

            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    item.UnitID = await _unitRepo.CreateUnit(item);
                    results.Add(item);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Unit_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UnitViewModel> list)
        {
            if (list != null && ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    await _unitRepo.UpdateUnit(item);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Unit_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UnitViewModel> list)
        {
            if (list.Any())
            {
                foreach (var item in list)
                {
                    await _publisherRepo.Delete(item.UnitID);
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}
