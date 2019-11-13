#pragma checksum "C:\Projects\ResearchApp\ResearchApp\Views\Home\_PartialRegionList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "23bdee376a5cdcc02167e60e3c3ea11f26f5b145"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home__PartialRegionList), @"mvc.1.0.view", @"/Views/Home/_PartialRegionList.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Projects\ResearchApp\ResearchApp\Views\_ViewImports.cshtml"
using ResearchApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Projects\ResearchApp\ResearchApp\Views\_ViewImports.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Projects\ResearchApp\ResearchApp\Views\Home\_PartialRegionList.cshtml"
using ResearchApp.Data;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"23bdee376a5cdcc02167e60e3c3ea11f26f5b145", @"/Views/Home/_PartialRegionList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ef318a74dc936a098c69682913676a2d60acfd18", @"/Views/_ViewImports.cshtml")]
    public class Views_Home__PartialRegionList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div class=\"grid-container ml-2\">\r\n    ");
#nullable restore
#line 3 "C:\Projects\ResearchApp\ResearchApp\Views\Home\_PartialRegionList.cshtml"
Write(Html.Kendo().Grid<ResearchApp.ViewModel.RegionViewModel>()
                  .Name("grid")
                  .Columns(columns =>
                  {
                      columns.Bound(b => b.RegionID).Width(100);
                      columns.Bound(b => b.Country).ClientTemplate("#=(Country.Option == null) ? '' : Country.Option#").Editable("checkIfGridEditable");
                      columns.Bound(b => b.Name).Editable("checkIfGridEditable");
                      columns.Bound(b => b.DisplayName).Editable("checkIfGridEditable");
                      columns.Bound(b => b.GeoNameCode).Editable("checkIfGridEditable");
                      columns.Bound(b => b.AlternateNames).Editable("checkIfGridEditable");
                      columns.Bound(b => b.GeoNameId).ClientTemplate("#=(GeoNameId == null) ? '' : GeoNameId#").Editable("checkIfGridEditable");
                      columns.Bound(b => b.NameAscii).Editable("checkIfGridEditable");
                      columns.Bound(b => b.Slug).Editable("checkIfGridEditable");
                      columns.Command(command => command.Destroy()).Width(100);
                  })
                  .ToolBar(toolbar =>
                  {
                      toolbar.Create().HtmlAttributes(new { @class = "d-none" });
                      toolbar.Save().HtmlAttributes(new { @class = "d-none" });
                  })
                  .Editable(editable => editable.Mode(GridEditMode.InCell))
                  .Pageable(page => page.ButtonCount(7).Responsive(true))
                  .Navigatable()
                  .Sortable()
                  .Filterable()
                  .Scrollable()
                  .Reorderable(x => x.Columns(true))
                  .ColumnMenu(col => col.Filterable(true).Enabled(true))
                  .Resizable(resize => resize.Columns(true))
                  .Events(e => e.DataBound("onDataBound").ColumnMenuInit("columnMenuInit"))
                  .DataSource(dataSource => dataSource
                      .Ajax()
                      .Batch(true)
                      .PageSize(1000)
                      .Events(events => events.Error("error_handler"))
                      .Sort(sort => sort.Add(s=> s.Name))
                      .Model(model =>
                      {

                          model.Id(p => p.RegionID);
                          model.Field(p => p.RegionID).Editable(false);
                          model.Field(field => field.Country).DefaultValue(new ResearchApp.ViewModel.DropdownOptions());
                      })
                      .Create("Region_Create", "Grid")
                      .Read("List", "Grid", new { type = GridTypes.Region })
                      .Update("Region_Update", "Grid")
                      .Destroy("Region_Destroy", "Grid")
                  )
    );

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
