using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LY.EMIS5.BLL;
using LY.EMIS5.Entities.Core.Memberships;
using NHibernate.Extensions.Data;
using System.Web.Security;
using LY.EMIS5.Common;
using System.Data;
using NHibernate.Extensions;
using LY.EMIS5.Common.Extensions;
using LY.EMIS5.Common.Mvc.Extensions;

namespace LY.EMIS5.Admin.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet, Authorize]
        public ActionResult Index()
        {
            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind != "管理员").AsSelectItemList(c => c.Id, c => c.Name);
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Index(Work work,int managerId)
        {
            work.CreateManager = ManagerImp.Current;
            work.WorkManager = DbHelper.Get<Manager>(managerId);
            work.CreateDate = DateTime.Now;
            work.State = 0;
            work.Save(true);
            return this.RedirectToAction(100, "操作成功", "工作安排成功!", "Home", "Index");
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

        [HttpGet, Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost, Authorize]
        public void Upload(string name)
        {
            DataTable dt = Util.ImportFromXls(Request.Files[0]);
            using (var ts = TransactionScopes.Default)
            {
                switch (name)
                {
                    case "业绩":
                        foreach (DataRow row in dt.Rows)
                        {
                            new Achievement { Company = row["公司"].ToString(), CreateDate = DateTime.Now, EndDate = DateTime.Parse(row["竣工时间"].ToString()), Manager = ManagerImp.Current, ProjectManager = row["项目经理"].ToString(), ProjectName = row["项目名称"].ToString(), Scale = row["规模"].ToString(), StartDate = DateTime.Parse(row["开工时间"].ToString()), Type = row["类型"].ToString() }.Save();
                        }
                        break;
                    case "资质":
                        foreach (DataRow row in dt.Rows)
                        {
                            new Aptitude { Company = row["公司"].ToString(), CreateDate = DateTime.Now, Level = row["等级"].ToString(), Manager = ManagerImp.Current, Name = row["资质名"].ToString() }.Save();
                        }
                        break;
                    case "证书":
                        foreach (DataRow row in dt.Rows)
                        {
                            new Certificate { AnnualVerificationDate = DateTime.Parse(row["年审时间"].ToString()), Company = row["公司"].ToString(), CreateDate = DateTime.Now, Major = row["专业"].ToString(), Manager = ManagerImp.Current, Name = row["姓名"].ToString(), Post = row["岗位"].ToString(), Remarks = row["备注"].ToString() }.Save();
                        }
                        break;
                    case "备案":
                        foreach (DataRow row in dt.Rows)
                        {
                            new Records { Area = row["区域"].ToString(), Company = row["公司"].ToString(), CreateDate = DateTime.Now, Date = DateTime.Parse(row["备案时间"].ToString()), IsRecord = row["是否备案"].ToString() == "是" ? true : false, Manager = ManagerImp.Current, Password = row["密码"].ToString(), Phone = row["电话"].ToString(), Remarks = row["备注"].ToString(), Situation = row["备案情况"].ToString(), Username = row["登录名"].ToString(), WebSite = row["网址"].ToString() }.Save();
                        }
                        break;
                    default:
                        break;
                }

                ts.Complete();
            }
        }

        public ActionResult Open() {
            var list = DbHelper.Query<Project>(c => c.OpenManager.Id == ManagerImp.Current.Id).Select(c => new { title = c.ProjectName, start = c.OpenDate.ToChineseDateString(), content = "项目" + c.ProjectName + "开标", user = c.Sale.Name,color = "#257e4a" }).ToList();
            if (list == null)
                list = DbHelper.Query<Work>(c => c.WorkManager.Id == ManagerImp.Current.Id).Select(c => new { title = c.Content, start = c.Date.ToChineseDateString(), content = c.Content, user = c.CreateManager.Name, color = "" }).ToList();
            else
                list.AddRange(DbHelper.Query<Work>(c => c.WorkManager.Id == ManagerImp.Current.Id).Select(c => new { title = c.Content, start = c.Date.ToChineseDateString(), content = c.Content, user = c.CreateManager.Name, color = "" }).ToList());
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}
