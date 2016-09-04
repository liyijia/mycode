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
    public class BidProjectController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            ViewBag.Companys = DbHelper.Query<Company>().AsSelectItemList(c => c.Name, c => c.Name);
            ViewBag.ProjectProgresses = EnumHelper<BidProjectProgresses>.EnumToSelectList();
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", string company = "", int state=0, int iDisplayStart = 0, int iDisplayLength = 15, int iSortCol_0 = 5,string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<BidProject> query = DbHelper.Query<BidProject>();
            if (!string.IsNullOrEmpty(company))
            {
                query = query.Where(c => c.Company.Contains(company));
            }
            if (!string.IsNullOrWhiteSpace(txt))
            {
                query = query.Where(c => c.ProjectName.Contains(txt));
            }
            if (state > 0)
            {
                query = query.Where(c => c.ProjectProgress == (BidProjectProgresses)state);
            }
            if (iSortCol_0 == 5)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.BidDate);
                }
                else {
                    query = query.OrderBy(c => c.BidDate);
                }
            }
            else if (iSortCol_0 == 0)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.ProjectProgress);
                }
                else {
                    query = query.OrderBy(c => c.ProjectProgress);
                }
            }
            else {
                query = query.OrderByDescending(c => c.Id);
            }
           
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderBy(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    c.ProjectName,
                    c.ProjectType,
                    c.Scale,
                    c.Money,
                    BidDate= c.BidDate.ToYearMonthDayString(),
                    c.Address,
                    c.UserName,
                    c.TeletePhone,
                    c.ProjectManager,
                    ProjectProgress=c.ProjectProgress.GetDescription(),
                    c.Company,
                    Edit = c.CreateManager.Id==ManagerImp.Current.Id || ManagerImp.Current.Kind == "管理员"
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            ViewBag.Companys = DbHelper.Query<Company>().AsSelectItemList(c => c.Name, c => c.Name);
            ViewBag.ProjectProgresses = EnumHelper<BidProjectProgresses>.EnumToSelectList();
            if (id > 0) {
                return View(DbHelper.Get<BidProject>(id));
            }
            return View(new BidProject());
        }

        [HttpPost, Authorize]
        public ActionResult Create(BidProject entity)
        {
            if (entity.Id > 0)
            {
                entity.Update(true);
            }
            else {
                entity.CreateDate = DateTime.Now;
                entity.CreateManager = ManagerImp.Current;
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑中标项目成功!", "BidProject", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<BidProject>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除中标项目成功!", "BidProject", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<BidProject>(id));
        }

    }
}
