﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LY.EMIS5.BLL;
using LY.EMIS5.Entities.Core.Memberships;
using NHibernate.Extensions.Data;
using System.Text;
using System.Web.Security;
using LY.EMIS5.Common.Security;
using LY.EMIS5.Common;
using LY.EMIS5.Common.Const;
using Couchbase;
using Enyim.Caching.Memcached;
using System.Data;
using NHibernate.Extensions;
using LY.EMIS5.Const;

namespace LY.EMIS5.Admin.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet, Authorize]
        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.User = null;
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now;
            return RedirectToAction("Login", "Home");
        }


        private void WriteCookie(int managerId)
        {
            //将认证信息写入Cookie，Cookie的生存期为浏览器进程
            var authTicket = new FormsAuthenticationTicket(1, managerId.ToString(), DateTime.Now, DateTime.MaxValue, false, managerId.ToString());
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket)) { HttpOnly = true };
            Response.Cookies.Add(authCookie);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Name, string Password)
        {
            var manager = ManagerImp.Login(Name, Password);
            WriteCookie(manager.Id);
            return RedirectToAction("AuditList", "Project");
        }
       
    }
}
