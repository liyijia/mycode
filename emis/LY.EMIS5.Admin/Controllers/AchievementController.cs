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
    public class AchievementController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Achievement> query = DbHelper.Query<Achievement>();
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderBy(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    c.ProjectName,
                    c.Scale,
                    StartDate=c.StartDate.ToYearMonthDayString(),
                    EndDate=c.EndDate.ToYearMonthDayString(),
                    c.ProjectManager,
                    c.Type,
                    Edit = c.Manager.Id==ManagerImp.Current.Id
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            if (id > 0) {
                return View(DbHelper.Get<Achievement>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(Achievement entity)
        {
            if (entity.Id > 0)
            {
                var ent = DbHelper.Get<Achievement>(entity.Id);
                ent.EndDate = entity.EndDate;
                ent.ProjectManager = entity.ProjectName;
                ent.Scale = entity.Scale;
                ent.StartDate = entity.StartDate;
                ent.Type = entity.Type;
                
                ent.Update(true);
            }
            else {
                entity.CreateDate = DateTime.Now;
                entity.Manager = ManagerImp.Current;
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑业绩成功!", "Achievement", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Achievement>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除业绩成功!", "Achievement", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Achievement>(id));
        }

    }
}
