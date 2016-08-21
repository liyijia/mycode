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
        public string Index(string txt = "",string t="通知", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<News> query = DbHelper.Query<News>(c => c.Type == t);
            if (!string.IsNullOrWhiteSpace(txt))
                query = query.Where(m => m.Title.Contains(txt));
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    c.Title,
                    UserName = c.Manager.Name,
                    CreateDate = c.CreateDate.ToYearMonthDayString(),
                    Edit = c.Manager.Id == ManagerImp.Current.Id || ManagerImp.Current.Kind=="管理员"
                }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<News>(id));
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id = 0, string t = "")
        {
            if (id > 0)
            {
                return View(DbHelper.Get<News>(id));
            }
            return View(new News() { Type= t });
        }

        [HttpPost, Authorize]
        [ValidateInput(false)]
        public ActionResult Create(News entity)
        {
            if (entity.Id > 0)
            {
                var news = DbHelper.Get<News>(entity.Id);
                news.Title = entity.Title;
                news.Content = entity.Content;
                news.Update(true);
            }
            else {
                entity.CreateDate = DateTime.Now;
                entity.Manager = ManagerImp.Current;
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑成功!", "News", "Index?t="+ entity.Type);
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            var entity = DbHelper.Get<News>(id);
            if(entity!=null)
                entity.Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除成功!", "News", "Index?t=" + entity.Type);
        }


        public ActionResult Top()
        {
            return Json(DbHelper.Query<News>().Take(10).OrderByDescending(c => c.Id).Select(c => new { c.Id, c.Title }), JsonRequestBehavior.AllowGet);
        }
    }
}
