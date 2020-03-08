using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LinkShorter.Views.Dashboard
{
    public static class DashboardNavPages
    {

        public static string Statistics => "Statistics";

        public static string Links => "Links";

        public static string Users => "Users";

        public static string UsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Users);

        public static string StatisticsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Statistics);

        public static string ManageLinksNavClass(ViewContext viewContext) => PageNavClass(viewContext, Links);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }


    }
}
