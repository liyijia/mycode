using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using LY.EMIS5.Common;
using LY.EMIS5.Common.Extensions;
using LY.EMIS5.Common.Mvc;
using LY.EMIS5.Common.Security;
using LY.EMIS5.Entities.Core.Memberships;
using NHibernate.Extensions.Data;
using NHibernate.Extensions;
using System.Data;
using LY.EMIS5.Const;
using LY.EMIS5.BLL;
using LY.EMIS5.Entities.Core;
using LY.EMIS5.Common.Exceptions;
using LY.EMIS5.Common.Mvc.Extensions;

namespace LY.EMIS5.Admin.Controllers
{
    public class NewsController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<News> query = DbHelper.Query<News>(c => c.Type == "公告");
            if (!string.IsNullOrWhiteSpace(txt))
                query = query.Where(m => m.Title.Contains(txt));
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).OrderByDescending(c => c.Id).ToList().Select(c => new
                {
                    Id = c.Id,
                    c.Title,
                    UserName = c.Manager.Name,
                    CreateDate = c.CreateDate.ToYearMonthDayString(),
                    Edit = c.Manager.Id == ManagerImp.Current.Id
                }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<News>(id));
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id = 0)
        {
            if (id > 0)
            {
                return View(DbHelper.Get<News>(id));
            }
            return View(new News());
        }

        [HttpPost, Authorize]
        public ActionResult Create(News entity)
        {
            if (entity.Id > 0)
            {
                entity.Update(true);
            }
            else {
                entity.CreateDate = DateTime.Now;
                entity.Manager = ManagerImp.Current;
                entity.Type = "公告";
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑公告成功!", "News", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<News>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除公告成功!", "News", "Index");
        }


        public ActionResult Top()
        {
            return Json(DbHelper.Query<News>().Take(10).OrderByDescending(c => c.Id).Select(c => new { c.Id, c.Title }), JsonRequestBehavior.AllowGet);
        }
    }
}
