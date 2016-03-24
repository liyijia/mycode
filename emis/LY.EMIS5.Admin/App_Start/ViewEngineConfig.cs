using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LY.EMIS5.Common;
using LY.EMIS5.Common.Mvc;

namespace LY.EMIS5.Admin
{
    public class ViewEngineConfig
    {
        public static void RegisterViewEngines(ViewEngineCollection viewEngines)
        {
            //viewEngines.Clear();

            viewEngines.Add(new ViewEngine(""));
            viewEngines.Add(new ViewEngine("Report/"));
        }
    }
}