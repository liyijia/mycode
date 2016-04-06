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
    public class RecordsController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Records> query = DbHelper.Query<Records>();
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).OrderBy(c=>c.Id).ToList().Select(c => new
                {
                    Id = c.Id,
                    c.Area,
                    IsRecord=c.IsRecord?"是":"否",
                    c.WebSite,
                    c.Username,
                    c.Password,
                    c.Phone,
                    c.Situation,
                    Date=c.Date.ToYearMonthDayString(),
                    c.Remarks,
                    Edit = c.Manager.Id==ManagerImp.Current.Id
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            if (id > 0) {
                return View(DbHelper.Get<Records>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(Records entity)
        {
            if (entity.Id > 0)
            {
                entity.Update(true);
            }
            else {
                entity.CreateDate = DateTime.Now;
                entity.Manager = ManagerImp.Current;
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑备案成功!", "Records", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Records>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除备案成功!", "Records", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Records>(id));
        }
    }
}
