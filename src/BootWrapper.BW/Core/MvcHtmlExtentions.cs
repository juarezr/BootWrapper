namespace System.Web.Mvc
{
    public static class MvcHtmlExtenstions
    {
        // B--
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, object routeValues, string imagePath, string alt)
        {
            return ActionImage(html, action, routeValues, imagePath, alt, null);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controllerName, object routeValues, string imagePath, string alt)
        {
            return ActionImage(html, action, controllerName, routeValues, imagePath, alt, null);
        }

        // B--
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, object routeValues, string imagePath, string alt, bool newWindow)
        {
            return ActionImage(html, action, routeValues, imagePath, alt, null, newWindow);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controllerName, object routeValues, string imagePath, string alt, bool newWindow)
        {
            return ActionImage(html, action, controllerName, routeValues, imagePath, alt, null, newWindow);
        }

        // B--
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, object routeValues, string imagePath, string alt, string confirmMessage)
        {
            return ActionImage(html, action, routeValues, imagePath, alt, confirmMessage, false);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controllerName, object routeValues, string imagePath, string alt, string confirmMessage)
        {
            return ActionImage(html, action, controllerName, routeValues, imagePath, alt, confirmMessage, false);
        }
        // E--

        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, object routeValues, string imagePath, string alt, string confirmMessage, bool newWindow)
        {
            string controllerName = html.ViewContext.RouteData.Values["Controller"].ToString();
            return ActionImage(html, action, controllerName, routeValues, imagePath, alt, confirmMessage, newWindow);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controllerName, object routeValues, string imagePath, string alt, string confirmMessage, bool newWindow)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("alt", alt);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(action, controllerName, routeValues));
            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside

            if (!string.IsNullOrEmpty(confirmMessage))
                anchorBuilder.MergeAttribute("onclick", "return confirm('" + confirmMessage + "')");

            if (newWindow)
                anchorBuilder.MergeAttribute("target", "_blank");

            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }

        public static MvcHtmlString MyCheckBox(this HtmlHelper html, string id, string name, bool selected)
        {
            //<input id="Pago + id" type="checkbox" value="value" onchange="onChange" checked="selected ? 'checked' : ''" />

            return MyCheckBox(html, id, name, selected, true);
        }

        public static MvcHtmlString MyCheckBox(this HtmlHelper html, string id, string name, bool selected, bool enabled)
        {
            //<input id="Pago + id" type="checkbox" value="value" onchange="onChange" checked="selected ? 'checked' : ''" />

            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("id", id);            
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", "true");

            if (!enabled)
                builder.MergeAttribute("DISABLED", "DISABLED");

            if (selected)
            {                
                builder.MergeAttribute("checked", "checked");
            }

            string Html = builder.ToString(TagRenderMode.SelfClosing);

            return MvcHtmlString.Create(Html);
        }


        public static MvcHtmlString MyCheckBox(this HtmlHelper html, string name, string id, string value, bool selected, string onchange)
        {
            //<input id="Pago + id" type="checkbox" value="value" onchange="onChange" checked="selected ? 'checked' : ''" />

            var builder = new TagBuilder("input");
            builder.MergeAttribute("id", name + id);
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("value", value);
            if (selected)
                builder.MergeAttribute("checked", "checked");

            builder.MergeAttribute("onchange", onchange);

            string Html = builder.ToString(TagRenderMode.SelfClosing);

            return MvcHtmlString.Create(Html);
        }


        public static MvcHtmlString LinkEntity(this HtmlHelper html, Type type, string action, object id, object text)
        {
            //<a href="type/action/id">type.Name</a>

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", urlHelper.Action(action, type.Name, new { Id = id }));
            builder.SetInnerText(text.ToString());

            string Html = builder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(Html);
        }

        public static MvcHtmlString LinkEntity(this HtmlHelper html, string controler, string action, object id, object text)
        {
            //<a href="type/action/id">type.Name</a>

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", urlHelper.Action(action, controler, new { Id = id }));
            builder.SetInnerText(text.ToString());

            string Html = builder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(Html);
        }

        public static MvcHtmlString LinkActionControl(this HtmlHelper html, string text, string action, string controller)
        {
            string builder = "<a href='/" + controller + "/" + action + "'>" + text + "</a>";

            return MvcHtmlString.Create(builder);
        }
    }
}
