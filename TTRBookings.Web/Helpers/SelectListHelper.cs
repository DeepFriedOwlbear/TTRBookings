using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TTRBookings.Web.Helpers
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> PopulateList<TEntity>(IEnumerable<TEntity> items, Func<TEntity, string> textSelector)
            where TEntity : Core.BaseEntity
        {
            return PopulateList(items, textSelector, null);
        }

        public static List<SelectListItem> PopulateList<TEntity>(IEnumerable<TEntity> items, Func<TEntity, string> textSelector, Guid? selectedId)
            where TEntity : Core.BaseEntity
        {
            List<SelectListItem> populatedList = new List<SelectListItem>();

            foreach (var item in items)
            {
                populatedList.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = textSelector(item),
                    Selected = item.Id == selectedId
                });
            }
            return populatedList;
        }
    }
}
