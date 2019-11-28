using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes=AttributeName)]
    public class ActiveRouteTagHelper : TagHelper
    {
        public const string AttributeName = "is-active-route";
        public const string IgnoreAction = "is-ignore-action";
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }
        private IDictionary<string, string> _routeData;
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix ="asp-route-")]
        public IDictionary<string,string> RouteData
        {
            get => _routeData ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            set => _routeData = value;
        }
        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            bool isIgnoreAction = context.AllAttributes.TryGetAttribute(IgnoreAction, out _);
            if (IsActive(isIgnoreAction))
                MakeActive(output);
            output.Attributes.RemoveAll(AttributeName);
        }

        private void MakeActive(TagHelperOutput output)
        {
            var class_attribute = output.Attributes.FirstOrDefault(a => a.Name == "class");

            if (class_attribute is null)
            {
                output.Attributes.Add("class", "active");
            }
            else
            {
                output.Attributes.SetAttribute("class", class_attribute.Value is null 
                    ? "active" 
                    : class_attribute.Value + " active");
            }
        }

        private bool IsActive(bool isIgnoreAction)
        {
            var route_data = ViewContext.RouteData.Values;
            var current_controller = route_data["Controller"].ToString();
            var current_action = route_data["Action"].ToString();

            if (!isIgnoreAction && !string.IsNullOrWhiteSpace(Action) && !string.Equals(Action, current_action, StringComparison.OrdinalIgnoreCase))
                return false;
            if (!string.IsNullOrWhiteSpace(Controller) && !string.Equals(Controller, current_controller, StringComparison.OrdinalIgnoreCase))
                return false;
            foreach (var item in RouteData)
                if (!route_data.ContainsKey(item.Key) || route_data[item.Key].ToString() != item.Value)
                    return false;
            return true;
        }
    }
}
