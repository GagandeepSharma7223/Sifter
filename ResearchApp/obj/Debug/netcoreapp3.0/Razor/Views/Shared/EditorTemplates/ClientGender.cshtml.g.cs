#pragma checksum "C:\Projects\Sifter\ResearchApp\Views\Shared\EditorTemplates\ClientGender.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c586d1a853591b1335b38f2c47a61ed8de0f8de6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_EditorTemplates_ClientGender), @"mvc.1.0.view", @"/Views/Shared/EditorTemplates/ClientGender.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c586d1a853591b1335b38f2c47a61ed8de0f8de6", @"/Views/Shared/EditorTemplates/ClientGender.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ef318a74dc936a098c69682913676a2d60acfd18", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_EditorTemplates_ClientGender : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ResearchApp.ViewModel.TextDropdownOptions>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Projects\Sifter\ResearchApp\Views\Shared\EditorTemplates\ClientGender.cshtml"
Write(Html.Kendo().DropDownListFor(m => m)
        .DataValueField("Id")
        .DataTextField("Option")
        .BindTo((System.Collections.IEnumerable)ViewData["genders"])
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ResearchApp.ViewModel.TextDropdownOptions> Html { get; private set; }
    }
}
#pragma warning restore 1591
