using PlagiarismCore.Extensions;
using System.Web;
using System.Web.Mvc;

namespace PlagiarismCore
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalErrorHandlerAttribute());
        }
    }
}
