using LY.EMIS5.Common;
using LY.EMIS5.Common.Mvc.Attributes;
using System.Web;
using System.Web.Mvc;

namespace LY.EMIS5.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AccessFrequencyAtrribute { MaxAccessFrequency = 20 });
            filters.Add(new CloseConnectionOnResultExecutedAttribute());
            filters.Add(new LY.EMIS5.Common.Mvc.Attributes.HandleErrorAttribute());
        }
    }
}