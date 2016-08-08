using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net.Config;
using LY.EMIS5.BLL;
using NHibernate.Extensions.Data;
using LY.EMIS5.Entities.Core;
using NHibernate.Extensions;
using NHibernate.Extensions.NamingStrategy;
using System.Web.Security;
using System.Xml.Linq;
using LY.EMIS5.Common.Utilities;
using LY.EMIS5.Entities.Core.Memberships;
using Newtonsoft.Json;
using LY.EMIS5.Admin.Models;

namespace LY.EMIS5.Admin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static int _InterceptsLoaded = 0;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ViewEngineConfig.RegisterViewEngines(ViewEngines.Engines);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            if (Interlocked.CompareExchange(ref _InterceptsLoaded, 1, 0) == 0)
            {
                //初始化拦截器和命名策略
                DbHelper.Configure(new IDynamicNamingStrategy[] { SchoolNamingStrategy.Instance },  typeof(Manager).Assembly);
                //启用日志
                XmlConfigurator.Configure(new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "")) + "\\log4net.cfg.xml"));
                var json = File.ReadAllText(Server.MapPath("/") + "json.json");
                HttpRuntime.Cache.Insert("json", json);
                var flow = JsonConvert.DeserializeObject<Flow>(json);
                flow.setDic();
                HttpRuntime.Cache.Insert("flow", flow);
            }

        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (Context.Request.Url.AbsolutePath.Equals(FormsAuthentication.LoginUrl, StringComparison.CurrentCultureIgnoreCase))
                return;
            var ext = Path.GetExtension(Context.Request.Url.AbsolutePath);
            if (!string.IsNullOrWhiteSpace(ext) && !ext.Equals(".aspx", StringComparison.CurrentCultureIgnoreCase)) //带后缀的文件为资源文件，不需要认证
                return;
            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !string.IsNullOrWhiteSpace(authTicket.Name))
                {
                    var user = ManagerImp.Get(Convert.ToInt32(new FormsIdentity(authTicket).Name));
                    Context.User = user;
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError().GetBaseException();
            
            LogUtils.Logger.Error(exception);
        }
    }
}