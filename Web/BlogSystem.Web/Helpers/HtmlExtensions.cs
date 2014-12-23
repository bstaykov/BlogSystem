namespace BlogSystem.Web.Helpers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class HtmlExtensions
    {
        public static MvcHtmlString Submit(this HtmlHelper helper, string value = "Submit")
        {
            var input = new TagBuilder("input");
            input.Attributes.Add("value", value);
            input.Attributes.Add("type", "submit");
            return new MvcHtmlString(input.ToString());
        }

        public static MvcHtmlString Submit(this HtmlHelper helper, string value = "Submit", IDictionary<string, string> htmlAttributes = null)
        {
            var input = new TagBuilder("input");
            input.MergeAttributes(htmlAttributes);
            input.Attributes.Add("value", value);
            input.Attributes.Add("type", "submit");
            return new MvcHtmlString(input.ToString());
        }

        public static MvcHtmlString Submit(this HtmlHelper helper, string value = "Submit", string @class = null)
        {
            var input = new TagBuilder("input");
            input.Attributes.Add("value", value);
            input.Attributes.Add("type", "submit");
            input.Attributes.Add("class", @class);
            return new MvcHtmlString(input.ToString());
        }

        public static MvcHtmlString Submit(this HtmlHelper helper, string value = "Submit", object htmlAttributes = null)
        {
            var input = new TagBuilder("input");
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) as IDictionary<string, object>;
            input.MergeAttributes(attributes);
            input.Attributes.Add("value", value);
            input.Attributes.Add("type", "submit");
            return new MvcHtmlString(input.ToString());
        }

        public static MvcHtmlString Span(this HtmlHelper helper, object htmlAttributes = null)
        {
            var span = new TagBuilder("span");
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) as IDictionary<string, object>;
            span.MergeAttributes(attributes);
            span.InnerHtml = "Namee";
            return new MvcHtmlString(span.ToString());
        }
    }
}