#pragma checksum "C:\Projects\Sifter\ResearchApp\Views\Shared\EditorTemplates\ClientCity.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b7032f80affe9220037cb44664c6c30ff9a468e3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_EditorTemplates_ClientCity), @"mvc.1.0.view", @"/Views/Shared/EditorTemplates/ClientCity.cshtml")]
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
#line 1 "C:\Projects\Sifter\ResearchApp\Views\_ViewImports.cshtml"
using ResearchApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Projects\Sifter\ResearchApp\Views\_ViewImports.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b7032f80affe9220037cb44664c6c30ff9a468e3", @"/Views/Shared/EditorTemplates/ClientCity.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ef318a74dc936a098c69682913676a2d60acfd18", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_EditorTemplates_ClientCity : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ResearchApp.ViewModel.DropdownOptions>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Projects\Sifter\ResearchApp\Views\Shared\EditorTemplates\ClientCity.cshtml"
Write(Html.Kendo().DropDownListFor(m => m)
                      .DataValueField("Id")
                      .DataTextField("Option")
                      .Filter(FilterType.Contains)
                      .MinLength(3)
                      .Height(200)
                      .DataSource(source =>
                      {
                          source.Custom()
                                .ServerFiltering(true)
                                .ServerPaging(true)
                                .PageSize(20)
                                .Type("aspnetmvc-ajax")
                                .Transport(transport =>
                                {
                                    transport.Read("GetDropdownOptions", "Grid", new { treeTable = "City", optionCol = "Name" });
                                })
                                .Schema(schema =>
                                {
                                    schema.Data("Data")
                                          .Total("Total");
                                });
                      })
                      .Virtual(v => v.ItemHeight(26).ValueMapper("valueMapperCity"))

);

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ResearchApp.ViewModel.DropdownOptions> Html { get; private set; }
    }
}
#pragma warning restore 1591
