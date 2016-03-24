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
using NHibernate.Linq;
using LY.EMIS5.Common.Exceptions;

namespace LY.EMIS5.Admin.Controllers
{
    public class ProjectController : Controller
    {


        [HttpGet]
        public ActionResult Index()
        {
            var list = DbHelper.Query<Manager>(c => c.Kind == "业务员").AsSelectItemList(c => c.Id, c => c.Name);
            list.Insert(0, new SelectListItem() { Text = "请选择业务员", Value = "0" });
            ViewBag.Sales = list;
            return View();
        }

        [HttpPost]
        public string Index(int iDisplayStart = 0, int iDisplayLength = 15, string name = "", int sale = 0, string state = "", string sEcho = "")
        {
            IQueryable<Project> query = DbHelper.Query<Project>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.ProjectName.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                query = query.Where(c => c.ProjectProgress == state);
            }
            if (sale > 0)
            {
                query = query.Where(c => c.Sale.Id == sale);
            }

            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).OrderBy(c => c.State).OrderBy(c => c.OpenDate).ToList().Select(c => new
                {
                    Id = c.Id,
                    ProjectName = c.ProjectName,
                    Name = c.Sale.Name,
                    c.Scale,
                    c.Money,
                    c.Source,
                    c.ProjectProgress,
                    OpenDate = c.OpenDate.ToYearMonthDayString(),
                    EndDate = c.EndDate.ToYearMonthDayString(),
                    c.CompanyName,
                    Prompt = (c.OpenDate - DateTime.Now).Days < 30
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        [HttpGet]
        public ActionResult AuditList()
        {
            return View();
        }

        [HttpPost]
        public string AuditList(int iDisplayStart = 0, int iDisplayLength = 15, string sEcho = "")
        {
            IQueryable<Project> query = DbHelper.Query<Project>(c => c.Sale.Id == ManagerImp.Current.Id || c.Opinions.Any(m => m.Manager.Id == ManagerImp.Current.Id && !m.Done));
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).OrderByDescending(c => c.CreateDate).ToList().Select(c => new
                {
                    Id = c.Id,
                    ProjectName = c.ProjectName,
                    Name = c.Sale.Name,
                    CreateDate = c.CreateDate.ToChineseDateString(),
                    ProjectProgress = c.ProjectProgress,
                    EndDate = c.EndDate.ToShortDateString(),
                    OpenDate = c.OpenDate.ToShortDateString(),
                    Edit = c.ProjectProgress == "未上网" && ManagerImp.Current.Id == c.Sale.Id,
                    Revoke = ManagerImp.Current.Id == c.Sale.Id,
                    Audit = c.Opinions.Any(m => m.Manager.Id == ManagerImp.Current.Id && !m.Done),
                    Prompt = (c.OpenDate - DateTime.Now).Days < 30
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        [HttpGet]
        public ActionResult Create(int id = 0)
        {

            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "资料员").AsSelectItemList(c => c.Id, c => c.Name);
            if (id > 0)
            {
                return View(DbHelper.Get<Project>(id));
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Project model, string CompanyNames, int documentId = 0)
        {
            foreach (var item in CompanyNames.Split(new string[] { ","},StringSplitOptions.None))
            {
                var entity = new Project();
                entity.Account = model.Account;
                entity.Bank = model.Bank;
                entity.CompanyName = item;
                entity.EndDate = model.EndDate;
                entity.Link = model.Link;
                entity.MaterialFee = model.MaterialFee;
                entity.Money = model.Money;
                entity.OpenDate = model.OpenDate;
                entity.Owner = model.Owner;
                entity.ProjectName = model.ProjectName;
                entity.Scale = model.Scale;
                entity.Source = model.Source;
                entity.Type = model.Type;
                entity.UserName = model.UserName;
                entity.SalesOpinion = model.SalesOpinion;
                if (entity.Id > 0)
                {
                    entity.Update(true);
                }
                else
                {
                    model.Sale = ManagerImp.Current;
                    model.CreateDate = DateTime.Now;
                    entity.Save(true);
                }
                if (entity.ProjectProgress != "未上网")
                {
                    new Opinion { Agree = false, CreateDate = DateTime.Now, Done = false, Manager = DbHelper.Get<Manager>(documentId), Project = entity, ProjectProgress = "做资料" }.Save();
                }
            }
            throw new AlertException(100, "操作成功", "保存项目成功", "Project", "Index");
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Project>(id));
        }

        [HttpGet]
        public ActionResult Audit(int id)
        {
            var entity = DbHelper.Get<Project>(id).Opinions.FirstOrDefault(c => !c.Done);
            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == entity.Kind).AsSelectItemList(c => c.Id, c => c.Name);
            return View(entity);
        }

        [HttpPost]
        public ActionResult Audit(int opinionid, string opinion, bool agree, int managerId = 0)
        {
            using (var ts = TransactionScopes.Default)
            {
                var entity = DbHelper.Get<Opinion>(opinionid);
                if (agree)
                {
                    entity.Project.ProjectProgress = entity.ProjectProgress;
                    if (managerId > 0)
                    {
                        Opinion op = new Opinion { Agree = false, CreateDate = DateTime.Now, Done = false, Manager = DbHelper.Get<Manager>(managerId), Project = entity.Project };
                        if (entity.ProjectProgress == "做资料")
                        {
                            op.ProjectProgress = "打保证金";
                            op.Kind = "财务";
                        }
                        else if (entity.ProjectProgress == "打保证金")
                        {
                            op.ProjectProgress = "同意开标";
                            op.Kind = "总经理";
                        }
                        else if (entity.ProjectProgress == "同意开标")
                        {
                            op.ProjectProgress = "开标结束";
                            op.Kind = "总经理";
                        }
                        else if (entity.ProjectProgress == "开标结束")
                        {
                            op.ProjectProgress = "项目结束";
                            op.Kind = "财务";
                        }
                        op.Save();
                    }
                }
                else
                {
                    entity.Project.ProjectProgress = "不能投标";
                }
                entity.Done = true;
                entity.Agree = agree;
                entity.Update();
                entity.Project.Update();
                ts.Complete();
            }
            throw new AlertException(100, "操作成功", "审核项目成功", "Project", "AuditList");
        }


        public ActionResult SearchProject(string term)
        {
            return Json(DbHelper.Query<Project>(c => c.ProjectName.Contains(term)).Select(c => c.ProjectName), JsonRequestBehavior.AllowGet);
        }
    }

}
