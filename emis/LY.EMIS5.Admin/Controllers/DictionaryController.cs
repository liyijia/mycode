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
using LY.EMIS5.Entities.Core.Stock;

namespace LY.EMIS5.Admin.Controllers
{
    public class DictionaryController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Dictionary> query = DbHelper.Query<Dictionary>();
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new { c.Id,c.Name, Manager=c.Manager.Name }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "项目经理").AsSelectItemList(c => c.Id, c => c.Name);
            if (id > 0) {
                return View(DbHelper.Get<Dictionary>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(Dictionary entity)
        {
            if (entity.Id > 0)
            {
                entity.Update(true);
            }
            else {
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑项目成功!", "Dictionary", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Dictionary>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除项目成功!", "Dictionary", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Dictionary>(id));
        }

    }
}
