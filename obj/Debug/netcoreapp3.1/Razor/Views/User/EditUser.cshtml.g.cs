#pragma checksum "C:\Project\NutsStore\Views\User\EditUser.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "33ac616417eb1bfaab009419b4a948b31121a634"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_EditUser), @"mvc.1.0.view", @"/Views/User/EditUser.cshtml")]
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
#line 1 "C:\Project\NutsStore\Views\_ViewImports.cshtml"
using NutsStore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Project\NutsStore\Views\_ViewImports.cshtml"
using NutsStore.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Project\NutsStore\Views\_ViewImports.cshtml"
using React.AspNet;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"33ac616417eb1bfaab009419b4a948b31121a634", @"/Views/User/EditUser.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"65516f1674939dae8e07be84ea7a741634d76fc6", @"/Views/_ViewImports.cshtml")]
    public class Views_User_EditUser : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<NutsStore.Models.UserInfo>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "0", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
  
    Layout = "~/Views/Layout.cshtml";
    var genderIndex = Model.Gender == true ? 0 : 1;

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"register-panel\">\r\n    <div id=\"bg\"></div>\r\n    <div class=\"form\">\r\n        <h5 class=\"title\">ویرایش کاربر</h5>\r\n        <div class=\"form-field\">\r\n            <input type=\"text\" placeholder=\"نام کاربری\"");
            BeginWriteAttribute("value", " value=\"", 348, "\"", 371, 1);
#nullable restore
#line 12 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
WriteAttributeValue("", 356, Model.Username, 356, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n\r\n        <div class=\"form-field\">\r\n            <input type=\"text\" placeholder=\"نام\"");
            BeginWriteAttribute("value", "  value=\"", 477, "\"", 502, 1);
#nullable restore
#line 16 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
WriteAttributeValue("", 486, Model.Firstname, 486, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n\r\n        <div class=\"form-field\">\r\n            <input type=\"text\" placeholder=\"نام خانوادگی\"");
            BeginWriteAttribute("value", "  value=\"", 617, "\"", 641, 1);
#nullable restore
#line 20 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
WriteAttributeValue("", 626, Model.Lastname, 626, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n\r\n        <div class=\"form-field\">\r\n            <input type=\"email\" placeholder=\"ایمیل\"");
            BeginWriteAttribute("value", "  value=\"", 750, "\"", 771, 1);
#nullable restore
#line 24 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
WriteAttributeValue("", 759, Model.Email, 759, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n\r\n        <div class=\"form-field\">\r\n            <input type=\"text\" placeholder=\"نشانی\"");
            BeginWriteAttribute("value", "  value=\"", 879, "\"", 902, 1);
#nullable restore
#line 28 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
WriteAttributeValue("", 888, Model.Address, 888, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n\r\n        <div class=\"form-field\">\r\n            <section>\r\n                <div class=\"gender-label\">جنسیت</div>\r\n                <select class=\"select-gender\"");
            BeginWriteAttribute("value", "  value=\"", 1083, "\"", 1104, 1);
#nullable restore
#line 34 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
WriteAttributeValue("", 1092, genderIndex, 1092, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "33ac616417eb1bfaab009419b4a948b31121a6346923", async() => {
                WriteLiteral("مرد");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "33ac616417eb1bfaab009419b4a948b31121a6348099", async() => {
                WriteLiteral("زن");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </select>\r\n            </section>\r\n        </div>\r\n\r\n        <div class=\"form-field\">\r\n            <input type=\"text\" placeholder=\"شماره تلفن\"");
            BeginWriteAttribute("value", " value=\"", 1369, "\"", 1395, 1);
#nullable restore
#line 42 "C:\Project\NutsStore\Views\User\EditUser.cshtml"
WriteAttributeValue("", 1377, Model.PhoneNumber, 1377, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n\r\n        <div class=\"form-field\">\r\n            <button class=\"btn\" type=\"submit\">ویرایش</button>\r\n        </div>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<NutsStore.Models.UserInfo> Html { get; private set; }
    }
}
#pragma warning restore 1591
