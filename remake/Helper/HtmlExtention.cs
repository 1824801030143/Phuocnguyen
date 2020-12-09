using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace remake.Helper
{
    public static class HtmlExtention
    {
        public static MvcHtmlString Image(this HtmlHelper html, byte[] image, string className, object htmlAtrributes = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("class", className);
            builder.MergeAttributes(new RouteValueDictionary(htmlAtrributes));

            var imageString = image != null ? Convert.ToBase64String(image) : "";
            var img = string.Format("data:image/jpg;base64,{0}", imageString);
            builder.MergeAttribute("src", img);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}