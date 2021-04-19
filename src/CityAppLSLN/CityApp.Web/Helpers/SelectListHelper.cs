using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CityApp.Web.Helpers
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> GetSelectedItems<T>(this List<T> list, 
            Func<T, string> text,
            Func<T, string> value, 
            Func<T, bool> selected = null)
        {
            var currentList = new List<SelectListItem>();
            foreach (var currentItem in list)
            {
                var selectListItem = new SelectListItem
                {
                    Text = text.Invoke(currentItem),
                    Value = value.Invoke(currentItem),
                    Selected = selected?.Invoke(currentItem) ?? false
                };
                currentList.Add(selectListItem);
            }

            return currentList;
        }

        public static MultiSelectList GetSelectedItems(this MultiSelectList list,
            List<string> toCompare)
        {
            foreach (var match in list)
            {
                match.Selected = toCompare.Contains(match.Value);
            }

            return list;
        }
    }
}