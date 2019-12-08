using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Models;

namespace WebStore.TagHelpers
{
    public class PagingTagHelper: TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName("page-model")]
        public PageViewModel PageModel { get; set; }
        [HtmlAttributeName("page-action")]
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url_helper = urlHelperFactory.GetUrlHelper(ViewContext);

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            for (var i = 1; i <= PageModel.TotalPages; i++)
                ul.InnerHtml.AppendHtml(CreateItem(i, url_helper));

            output.Content.AppendHtml(ul);
        }

        private IHtmlContent CreateItem(int PageNumber, IUrlHelper url_helper)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");

            if (PageNumber == PageModel.PageNumber)
            {
                a.MergeAttribute("data-page", PageModel.PageNumber.ToString());
                li.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = PageNumber;
                a.Attributes["href"] = "#";//url_helper.Action(PageAction, PageUrlValues);
                foreach (var (key, value) in PageUrlValues.Where(p=>p.Value!=null))
                {
                    a.MergeAttribute($"data-{key}", value.ToString());
                }
            }

            a.InnerHtml.AppendHtml(PageNumber.ToString());
            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}
