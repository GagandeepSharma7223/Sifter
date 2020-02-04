using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ResearchApp.Data;
using ResearchApp.Extension;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
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
        private readonly IMemoryCache _cache;

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
        public async Task<IActionResult> Work_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkViewModel> works)
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
        public async Task<IActionResult> Work_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkViewModel> works)
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
        public async Task<IActionResult> Work_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<WorkViewModel> works)
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
        public async Task<IActionResult> Author_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AuthorViewModel> authors)
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
        public async Task<IActionResult> Author_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AuthorViewModel> authors)
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
        public async Task<IActionResult> Author_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AuthorViewModel> authors)
        {
            if (authors.Any())
            {
                foreach (var author in authors)
                {
                    await _authorRepo.Delete(author.AuthorID.GetValueOrDefault());

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

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Publisher_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PublisherViewModel> publishers)
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
        public async Task<IActionResult> Publisher_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PublisherViewModel> publishers)
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
        public async Task<IActionResult> Publisher_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PublisherViewModel> publishers)
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
                    PopulateRoles();
                    view = "~/Views/Home/_PartialWorkAuthorList.cshtml";
                    break;
                case GridTypes.Unit:
                    view = "~/Views/Home/_PartialUnitList.cshtml";
                    break;
            }
            return PartialView(view);
        }

        public async Task<IActionResult> GetFormView(GridTypes type, string selectedItem)
        {
            string view = "~/Views/Home/_PartialWorkForm.cshtml";
            dynamic item = selectedItem != null ? JsonConvert.DeserializeObject<Dictionary<string, object>>(selectedItem) : null;
            var controls = await _workRepo.GetTreeColumnsForTable(type.ToString());
            var formViewModel = new FormViewModel
            {
                SelectedItem = item,
                TableColumns = controls
            };
            return PartialView(view, formViewModel);
        }

        public async Task<IActionResult> GetSearchForm(GridTypes type)
        {
            var controls = await _workRepo.GetTreeColumnsForTable(type.ToString());
            var formViewModel = new FormViewModel
            {
                TableColumns = controls,
                FormName = type.ToString()
            };
            return PartialView("~/Views/Home/_PartialSearchForm.cshtml", formViewModel);
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> SaveForm(GridTypes type, string selectedItem)
        {
            int id = 0;
            switch (type)
            {
                case GridTypes.Category:
                    var category = JsonConvert.DeserializeObject<CategoryViewModel>(selectedItem);
                    if (category.CategoryID.HasValue)
                    {
                        id = category.CategoryID.Value;
                        await _categoryRepo.UpdateCategory(category, true);
                    }
                    else
                    {
                        id = await _categoryRepo.CreateCategory(category, true);
                    }
                    break;
                case GridTypes.Language:
                    var language = JsonConvert.DeserializeObject<LanguageViewModel>(selectedItem);
                    if (language.LanguageID.HasValue)
                    {
                        id = language.LanguageID.Value;
                        await _languageRepo.UpdateLanguage(language, true);
                    }
                    else
                    {
                        id = await _languageRepo.CreateLanguage(language, true);
                    }
                    break;
                case GridTypes.City:
                    var city = JsonConvert.DeserializeObject<CityViewModel>(selectedItem);
                    if (city.CityID.HasValue)
                    {
                        id = city.CityID.Value;
                        await _cityRepo.UpdateCity(city, true);
                    }
                    else
                    {
                        id = await _cityRepo.CreateCity(city, true);
                    }
                    break;
                case GridTypes.Region:
                    var region = JsonConvert.DeserializeObject<RegionViewModel>(selectedItem);
                    if (region.RegionID.HasValue)
                    {
                        id = region.RegionID.Value;
                        await _regionRepo.UpdateRegion(region, true);
                    }
                    else
                    {
                        id = await _regionRepo.CreateRegion(region, true);
                    }
                    break;
                case GridTypes.Country:
                    var country = JsonConvert.DeserializeObject<CountryViewModel>(selectedItem);
                    if (country.CountryID.HasValue)
                    {
                        id = country.CountryID.Value;
                        await _countryRepo.UpdateCountry(country, true);
                    }
                    else
                    {
                        id = await _countryRepo.CreateCountry(country, true);
                    }
                    break;
                case GridTypes.Publisher:
                    var publisher = JsonConvert.DeserializeObject<PublisherViewModel>(selectedItem);
                    if (publisher.PublisherID.HasValue)
                    {
                        id = publisher.PublisherID.Value;
                        await _publisherRepo.UpdatePublisher(publisher, true);
                    }
                    else
                    {
                        id = await _publisherRepo.CreatePublisher(publisher, true);
                    }
                    break;
                case GridTypes.Work:
                    var work = JsonConvert.DeserializeObject<WorkViewModel>(selectedItem);
                    if (work.WorkID.HasValue)
                    {
                        id = work.WorkID.Value;
                        await _workRepo.UpdateWork(work, true);
                    }
                    else
                    {
                        id = await _workRepo.CreateWork(work, true);
                    }
                    break;
                case GridTypes.Author:
                    var author = JsonConvert.DeserializeObject<AuthorViewModel>(selectedItem);
                    if (author.AuthorID.HasValue)
                    {
                        id = author.AuthorID.Value;
                        await _authorRepo.UpdateAuthor(author, true);
                    }
                    else
                    {
                        id = await _authorRepo.CreateAuthor(author, true);
                    }
                    break;
                case GridTypes.WorkAuthor:
                    var workAuthor = JsonConvert.DeserializeObject<WorkAuthorViewModel>(selectedItem);
                    if (workAuthor.WorkAuthorID.HasValue)
                    {
                        id = workAuthor.WorkAuthorID.Value;
                        await _workAuthorRepo.UpdateWorkAuthor(workAuthor, true);
                    }
                    else
                    {
                        id = await _workAuthorRepo.CreateWorkAuthor(workAuthor, true);
                    }
                    break;
                case GridTypes.Unit:
                    var unit = JsonConvert.DeserializeObject<UnitViewModel>(selectedItem);
                    if (unit.UnitID.HasValue)
                    {
                        id = unit.UnitID.Value;
                        await _unitRepo.UpdateUnit(unit, true);
                    }
                    else
                    {
                        id = await _unitRepo.CreateUnit(unit, true);
                    }
                    break;
            }
            return Ok(id);
        }

        [AcceptVerbs("Post")]
        public IActionResult SearchForm(List<SearchParams> searchParams, AdvanceSearchRequest request)
        {
            try
            {
                if (request.PageSize == 0) request.PageSize = 1000;
                var dbResult = _authorRepo.PopulateSearchResult(searchParams.ToDataTable(), request);
                var response = new AdvanceSearchResult
                {
                    SearlizeResult = JsonConvert.SerializeObject(dbResult.ToDynamic()),
                    GridType = request.GridType
                };
                if (request.Total == 0)
                {
                    request.CountOnly = true;
                    var dbCountResult = _authorRepo.PopulateSearchResult(searchParams.ToDataTable(), request);
                    if (dbCountResult.Rows.Count > 0)
                    {
                        response.Total = (int)dbCountResult.Rows[0][0];
                    }
                }
                switch (request.GridType)
                {
                    case GridTypes.VAuthor:
                        response.Columns = _authorRepo.GetMetaColumns("vAuthor");
                        break;
                    case GridTypes.VWork:
                        response.Columns = _workRepo.GetMetaColumns("vWork");
                        break;
                    case GridTypes.VUnit:
                        response.Columns = _unitRepo.GetMetaColumns("vUnit");
                        break;
                };
                return PartialView("~/Views/Home/_PartialSearchResult.cshtml", response);
            }
            catch (Exception e)
            {
            }
            return Json(new { IsError = true });
        }

        public JsonResult AdvanceSearchResult(List<SearchParams> searchParams, AdvanceSearchRequest request)
        {
            var dbResult = _authorRepo.PopulateSearchResult(searchParams.ToDataTable(), request);
            return Json(JsonConvert.SerializeObject(dbResult.ToDynamic()));
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

        [AcceptVerbs("Post")]
        public IActionResult BindFilterListView([DataSourceRequest] DataSourceRequest request, string treeTable, string optionCol, string fieldType)
        {
            var (result, count) = _workRepo.GetFilterData(treeTable, optionCol, request, fieldType);
            var response = new DataSourceResult
            {
                Data = result,
                Total = count
            };
            return Json(response);
        }


        [AcceptVerbs("Post")]
        [ResponseCache(Duration = 3600)]
        public IActionResult GetDropdownOptions([DataSourceRequest] DataSourceRequest request, string treeTable, string optionCol)
        {
            if (!_cache.TryGetValue($"{treeTable}Options", out List<DropdownOptions> cachedDetails))
            {
                cachedDetails = _workRepo.GetOptions(treeTable, optionCol);
                cachedDetails.Insert(0, new DropdownOptions { Id = 0, Option = "" });
                _cache.Set($"{treeTable}Options", cachedDetails, CacheEntryOptions);
            }
            if (!cachedDetails.Any(x => x.Id == 0))
            {
                cachedDetails.Insert(0, new DropdownOptions { Id = 0, Option = "" });
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

        [AcceptVerbs("Post")]
        public IActionResult GenderOptions([DataSourceRequest] DataSourceRequest request)
        {
            List<TextDropdownOptions> genders = GetGenderOptions();
            return Json(genders.ToDataSourceResult(request));
        }


        private void PopulateGenders()
        {
            List<TextDropdownOptions> genders = GetGenderOptions();
            ViewData["genders"] = genders;
        }


        private void PopulateRoles()
        {
            List<TextDropdownOptions> roles = GetRoleOptions();
            ViewData["roles"] = roles;
        }

        private List<TextDropdownOptions> GetGenderOptions()
        {
            return new List<TextDropdownOptions>
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
        }

        private List<TextDropdownOptions> GetRoleOptions()
        {
            return new List<TextDropdownOptions>
            {
                new TextDropdownOptions{
                    Id = "",
                    Option = ""
                },
                new TextDropdownOptions{
                    Id = "Author",
                    Option = "Author"
                },
                new TextDropdownOptions{
                    Id = "Editor",
                    Option = "Editor"
                },
                new TextDropdownOptions{
                    Id = "Translator",
                    Option = "Translator"
                }
            };
        }

        [AcceptVerbs("Post")]
        public IActionResult YesNoOptions([DataSourceRequest] DataSourceRequest request)
        {
            List<TextDropdownOptions> options = GetYesNoOptions();
            return Json(options.ToDataSourceResult(request));
        }

        private void PopulateYesNo()
        {
            List<TextDropdownOptions> options = GetYesNoOptions();
            ViewData["yesNoOptions"] = options;
        }

        private static List<TextDropdownOptions> GetYesNoOptions()
        {
            return new List<TextDropdownOptions>
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
        }

        public JsonResult GetFKDisplayColumn(string tableName, string displayName)
        {
            string result = _workRepo.GetFKDisplayColumn(tableName, displayName);
            return Json(result);
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
                    await _categoryRepo.Delete(item.CategoryID.GetValueOrDefault());
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
                    await _languageRepo.Delete(item.LanguageID.GetValueOrDefault());
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
                    await _cityRepo.Delete(item.CityID.GetValueOrDefault());
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
                    await _regionRepo.Delete(item.RegionID.GetValueOrDefault());
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
                    await _countryRepo.Delete(item.CountryID.GetValueOrDefault());
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
                    await _workAuthorRepo.Delete(item.WorkAuthorID.GetValueOrDefault());
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
                    await _unitRepo.Delete(item.UnitID.GetValueOrDefault());
                }
            }
            return Json(list.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}
