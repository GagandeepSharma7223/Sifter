#pragma checksum "C:\Projects\Backup\ResearchApp\18Oct\ResearchApp\ResearchApp\Views\Shared\EditorTemplates\ClientAuthor.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3765f15013170a2e7a8fb3083c2999175204672d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_EditorTemplates_ClientAuthor), @"mvc.1.0.view", @"/Views/Shared/EditorTemplates/ClientAuthor.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/EditorTemplates/ClientAuthor.cshtml", typeof(AspNetCore.Views_Shared_EditorTemplates_ClientAuthor))]
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
#line 1 "C:\Projects\Backup\ResearchApp\18Oct\ResearchApp\ResearchApp\Views\_ViewImports.cshtml"
using ResearchApp;

#line default
#line hidden
#line 2 "C:\Projects\Backup\ResearchApp\18Oct\ResearchApp\ResearchApp\Views\_ViewImports.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3765f15013170a2e7a8fb3083c2999175204672d", @"/Views/Shared/EditorTemplates/ClientAuthor.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ef318a74dc936a098c69682913676a2d60acfd18", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_EditorTemplates_ClientAuthor : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ResearchApp.ViewModel.DropdownOptions>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(46, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(50, 1111, false);
#line 3 "C:\Projects\Backup\ResearchApp\18Oct\ResearchApp\ResearchApp\Views\Shared\EditorTemplates\ClientAuthor.cshtml"
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
                                transport.Read("GetDropdownOptions", "Grid", new { treeTable = "Author", optionCol = "FullName" });
                            })
                            .Schema(schema =>
                            {
                                schema.Data("Data")
                                      .Total("Total");
                            });
                  })
                  .Virtual(v => v.ItemHeight(26).ValueMapper("valueMapperAuthor"))

);

#line default
#line hidden
            EndContext();
            BeginContext(1162, 2, true);
            WriteLiteral("\r\n");
            EndContext();
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
