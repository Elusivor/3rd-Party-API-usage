using System.Web;
using System.Web.Mvc;

namespace _3rd_Party_API_usage
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
