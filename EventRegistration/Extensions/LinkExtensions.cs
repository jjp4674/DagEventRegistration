using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EventRegistration.Extensions
{
    public static class LinkExtensions
    {
        public static HtmlString NavigationLink(this HtmlHelper html, string linkText, string actionName, string controllerName)
        {
            string contextAction = (string)html.ViewContext.RouteData.Values["action"];
            string contextController = (string)html.ViewContext.RouteData.Values["controller"];

            bool isCurrent =
                string.Equals(contextAction, actionName, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(contextController, controllerName, StringComparison.CurrentCultureIgnoreCase);

            return html.ActionLink(
                linkText,
                actionName,
                controllerName,
                new { area = "" },
                htmlAttributes: isCurrent ? new { @class = "current" } : null
                );
        }

        public static HtmlString DropdownLink(this HtmlHelper html, string linkText, string controllerName, string id)
        {
            string contextAction = (string)html.ViewContext.RouteData.Values["action"];
            string contextController = (string)html.ViewContext.RouteData.Values["controller"];

            bool isCurrent =
                string.Equals(contextController, controllerName, StringComparison.CurrentCultureIgnoreCase);

            return html.ActionLink(
                linkText,
                "",
                "",
                null,
                htmlAttributes: isCurrent ?
                    new { @class = "current dropdown-toggle", @id = id, @role = "button", @data_toggle = "dropdown", @aria_haspopup = "true", @aria_expanded = "false" } :
                    new { @class = "dropdown-toggle", @id = id, @role = "button", @data_toggle = "dropdown", @aria_haspopup = "true", @aria_expanded = "false" }
                );
        }

    }
}