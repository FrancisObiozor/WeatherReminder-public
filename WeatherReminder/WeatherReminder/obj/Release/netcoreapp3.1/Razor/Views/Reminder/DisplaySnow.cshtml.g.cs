#pragma checksum "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d1611d9536ee6bb36cfffea2ce7f54470db686e2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Reminder_DisplaySnow), @"mvc.1.0.view", @"/Views/Reminder/DisplaySnow.cshtml")]
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
#line 1 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\_ViewImports.cshtml"
using WeatherReminder;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\_ViewImports.cshtml"
using WeatherReminder.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
using Newtonsoft.Json.Converters;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
using WeatherReminder.Areas.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d1611d9536ee6bb36cfffea2ce7f54470db686e2", @"/Views/Reminder/DisplaySnow.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a223f2166ced42eb39a727d6fc959b04ebb144a1", @"/Views/_ViewImports.cshtml")]
    public class Views_Reminder_DisplaySnow : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WeatherReminder.ViewModels.DisplayRemindersViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("role", new global::Microsoft.AspNetCore.Html.HtmlString("button"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
  
    ViewData["Title"] = "View";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div class=\"container\">\r\n");
#nullable restore
#line 13 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
     if (Model.Reminders.Count == 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <div class=""row mt-1 justify-content-center"">

        <div class=""col-md-3"" id=""ulContainer"">
            <h4 class=""mt-4 text-center"">Reminders</h4>

            <ul class=""list-group"">
                <li class=""list-group-item border border-dark border-1"">
                    <div class=""text-danger text-center"">You have no reminders.</div>
                </li>
            </ul>

            <div class=""mt-3 text-center"">
                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d1611d9536ee6bb36cfffea2ce7f54470db686e26929", async() => {
                WriteLiteral("Home");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n\r\n       \r\n    </div>\r\n");
#nullable restore
#line 33 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"row mt-1 justify-content-center\">\r\n\r\n            <div class=\"col-md-4\">\r\n                <h4 class=\"mt-4 text-center\">Reminders</h4>\r\n\r\n                <ul class=\"list-group\">\r\n");
#nullable restore
#line 42 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                     foreach (var remind in Model.Reminders)
                    {
                        string time = remind.ReminderTime.ToString("hh:mm tt");

                        if (remind.DaysBeforeEvent == 0)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li class=\"list-group-item text-center border border-dark border-1\">\r\n                                <div class=\"text-info inline\">Day of event</div> at <div class=\"inline text-info\">");
#nullable restore
#line 49 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                                                                                                             Write(time);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                            </li>\r\n");
#nullable restore
#line 51 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                        }
                        else if (remind.DaysBeforeEvent == 1)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li class=\"list-group-item text-center border border-dark border-1\">\r\n                                <div class=\"text-info inline\">");
#nullable restore
#line 55 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                                                         Write(remind.DaysBeforeEvent);

#line default
#line hidden
#nullable disable
            WriteLiteral(" day before Event</div> at <div class=\"inline text-info\">");
#nullable restore
#line 55 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                                                                                                                                         Write(time);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                            </li>\r\n");
#nullable restore
#line 57 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li class=\"list-group-item text-center border border-dark border-1\">\r\n                                <div class=\"text-info inline\">");
#nullable restore
#line 61 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                                                         Write(remind.DaysBeforeEvent);

#line default
#line hidden
#nullable disable
            WriteLiteral(" days before Event</div> at <div class=\"inline text-info\">");
#nullable restore
#line 61 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                                                                                                                                          Write(time);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                            </li>\r\n");
#nullable restore
#line 63 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
                        }

                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </ul>\r\n                \r\n                <div class=\"mt-3 text-center\">\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d1611d9536ee6bb36cfffea2ce7f54470db686e213657", async() => {
                WriteLiteral("Home");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n\r\n        </div>\r\n");
#nullable restore
#line 75 "C:\Users\fobio\OneDrive\Documents\Coding\C#\Personal Portfolio\Weather Reminder\Latest\WeatherReminder\WeatherReminder\Views\Reminder\DisplaySnow.cshtml"
    }

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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WeatherReminder.ViewModels.DisplayRemindersViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
