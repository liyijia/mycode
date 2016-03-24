using LY.EMIS5.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using System.Reflection;

namespace LY.EMIS5.Common.Mvc.Attributes
{
    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            if (!filterContext.Controller.ToString().Contains("API"))
            {
                if (filterContext == null)
                    throw new ArgumentNullException("filterContext");
                if (filterContext.HttpContext.Response.StatusCode == (int)System.Net.HttpStatusCode.NotFound)
                    return;

                StringBuilder error = new StringBuilder();
                foreach (var item in filterContext.RouteData.Values)
                {
                    error.AppendFormat("KEY:{0},VALUE:{1}；", item.Key, item.Value);
                }
                Util.GetLogger().Error("服务器发生意外错误" + error.ToString(), filterContext.Exception);
                if (filterContext.IsChildAction)
                    return;
                if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                    return;
                if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500 || !ExceptionType.IsInstanceOfType(filterContext.Exception))
                    return;
                if (!(filterContext.Exception is EmisException))
                    Util.GetLogger().Error("服务器发生意外错误。url:" + filterContext.HttpContext.Request.RawUrl, filterContext.Exception);

                filterContext.Exception = filterContext.Exception as EmisException ?? (filterContext.HttpContext.Request.IsAjaxRequest() ? new AjaxException(500, "操作失败，请重试", null, filterContext.Exception) : new EmisException(500, "服务器发生意外错误", "服务器发生意外错误", filterContext.Exception));
                filterContext.ExceptionHandled = true;
                string controllerName = "", actionName = "", title = ((EmisException)filterContext.Exception).Title;
                if (filterContext.Exception is AjaxException)
                {
                    //var exception = filterContext.Exception as AjaxException;
                    //exception.Data = null;
                    //new JsonResult
                    //{
                    //    ContentEncoding = Encoding.UTF8,
                    //    ContentType = "application/json",
                    //    JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                    //    Data = exception.ToAjaxObject()
                    //}.ExecuteResult(filterContext);
                    //return;
                    throw new HttpException(500, filterContext.Exception.Message);
                }
                else if (filterContext.Exception is AlertException)
                {
                    var exception = filterContext.Exception as AlertException;
                    controllerName = exception.ControllerName ?? (string)filterContext.RouteData.Values["controller"];
                    actionName = exception.ActionName ?? (string)filterContext.RouteData.Values["action"];
                    if (string.IsNullOrWhiteSpace(filterContext.Exception.HelpLink))
                        filterContext.Exception.HelpLink = UrlHelper.GenerateUrl(null, actionName, controllerName, exception.RouteValues == null ? null : new RouteValueDictionary(exception.RouteValues), RouteTable.Routes, filterContext.RequestContext, false);
                }
                else
                {
                    controllerName = (string)filterContext.RouteData.Values["controller"];
                    actionName = (string)filterContext.RouteData.Values["action"];
                    if (string.IsNullOrWhiteSpace(filterContext.Exception.HelpLink))
                        filterContext.Exception.HelpLink = filterContext.HttpContext.Request.Url.PathAndQuery;
                }
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                var actionResult = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
                actionResult.ViewBag.Title = title;
                actionResult.ExecuteResult(filterContext);
            }
        }
    }
}
