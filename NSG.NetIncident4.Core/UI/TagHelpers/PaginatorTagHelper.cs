// ===========================================================================
// File: PaginatorTagHelper.cs
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
//
using NSG.PrimeNG.LazyLoading;
using NSG.NetIncident4.Core.UI.ViewModels;
//
namespace NSG.NetIncident4.Core.UI.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    /// <summary>
    /// Tag helper pager using the PrimeNG's LazyLoadEvent2 class
    /// <note>
    /// Concept from the following article:
    /// https://gunnarpeipman.com/aspnet-core-pager-tag-helper/
    /// Code found on github:
    /// https://github.com/gpeipman/DotNetPaging
    /// </note>
    /// <example>
    ///   @model NSG.NetIncident4.Core.UI.ViewModels.Pagination<TestData>
    ///   ...
    ///   <paginator paginator-model="@Model"></paginator>
    /// </example>
    /// </summary>
    [HtmlTargetElement("paginator", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PaginatorTagHelper : TagHelper
    {
        private readonly HttpContext _httpContext;
        private readonly IUrlHelper _urlHelper;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public PaginatorTagHelper(IHttpContextAccessor accessor, IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _httpContext = accessor.HttpContext;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        [HtmlAttributeName("paginator-model")]
        public IPagination Model { get; set; }
        //
        /// <summary>
        /// Process the pager tag helper.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Model == null)
            {
                return;
            }
            if (Model.pageCount == 0 || Model.lazyLoadEvent == null)
            {
                return;
            }
            if (this.Model.pageCount > 1)
            {
                var start = Model.GetStartWindow();
                var end = Model.GetEndWindow();
                output.Content.AppendHtml("<ul class='pagination pagination justify-content-center nsg-hd-row'>");
                AddPageLink(output, CreateUrl(1), "&laquo;", Model.IsFirstPage());
                AddPageLink(output, CreateUrl(Model.pageIndex - 1), "&lt;", Model.IsFirstPage());
                for (var pg = start; pg <= end; pg++)
                {
                    AddPageLink(output, CreateUrl(pg), pg.ToString(), (pg == Model.pageIndex));
                }
                AddPageLink(output, CreateUrl(Model.pageIndex + 1), "&gt;", Model.IsLastPage());
                AddPageLink(output, CreateUrl(Model.pageCount), "&raquo;", Model.IsLastPage());
                output.Content.AppendHtml("</ul>");
            }
        }
        //
        private string CreateUrl(long pg)
        {
            LazyLoadEvent2 _param = Model.GetRouteForPage(pg);
            return _urlHelper.Action(Model.action, null, Model.GetRouteForPage(pg), null, null, null);
        }
        //
        private void AddPageLink(TagHelperOutput output, string url, string text, bool diabled)
        {
            output.Content.AppendHtml($"<li class='page-item{(diabled ? " disabled" : "")}' >");
            output.Content.AppendHtml($"<a class='page-link' href='{url}'>{text}</a>");
            output.Content.AppendHtml("</li>");
        }
        //
    }
}
// ===========================================================================
