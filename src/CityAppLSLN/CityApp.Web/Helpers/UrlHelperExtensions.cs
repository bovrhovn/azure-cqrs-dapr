using System;
using Microsoft.AspNetCore.Mvc;

namespace CityApp.Web.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string ToAbsoluteAction(
            this IUrlHelper url,
            string actionName,
            string controllerName,
            object routeValues = null)
        {
            return url.Action(actionName, controllerName, routeValues, url.ActionContext.HttpContext.Request.Scheme);
        }

        public static string ToAbsoluteContent(
            this IUrlHelper url,
            string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;
            var relativeUri = url.Content(contentPath);
            var currentUrl = new Uri(new Uri(request.Scheme + "://" + request.Host.Value), 
                relativeUri);
            return currentUrl.ToString();
        }

        public static string ToAbsoluteRouteUrl(
            this IUrlHelper url,
            string routeName,
            object routeValues = null)
        {
            return url.RouteUrl(routeName, routeValues, 
                url.ActionContext.HttpContext.Request.Scheme);
        }
    }
}