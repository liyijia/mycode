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

namespace LY.EMIS5.Admin.Controllers
{
    public class ProjectController : Controller
    {


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Index(int iDisplayStart = 0, int iDisplayLength = 15, string name = "", string state = "", string sEcho = "")
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
            switch (ManagerImp.Current.Kind)
            {
                case "业务员":
                    query = query.Where(c => c.Sale.Id == ManagerImp.Current.Id);
                    break;
                case "资料员":
                    query = query.Where(c => c.Documenter.Id == ManagerImp.Current.Id);
                    break;
                case "财务":
                    query = query.Where(c => c.Finance.Id == ManagerImp.Current.Id);
                    break;
                case "总经理":
                    query = query.Where(c => c.CEO.Id == ManagerImp.Current.Id);
                    break;
                case "开标人":
                    query = query.Where(c => c.OpenPeople.Id == ManagerImp.Current.Id);
                    break;
                default:
                    break;
            }

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
                    Edit = c.ProjectProgress == "已上网"&&(ManagerImp.Current.Kind == "管理员" || ManagerImp.Current.Id == c.Sale.Id),
                    Audit= IsAudit(c),
                    Prompt = (c.OpenDate-DateTime.Now).Days<30
                }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        private bool IsAudit(Project entity) {
            if (entity.ProjectProgress == "已上网")
            {
                return entity.Documenter.Id == ManagerImp.Current.Id;
            }
            else if (entity.ProjectProgress == "做资料")
            {
                return entity.Finance.Id == ManagerImp.Current.Id;
            }
            else if (entity.ProjectProgress == "打保证金")
            {
                return entity.CEO.Id == ManagerImp.Current.Id;
            }
            else if (entity.ProjectProgress == "开标结束")
            {
                return entity.OpenPeople.Id == ManagerImp.Current.Id;
            }
            return false;
        }

        [HttpGet]
        public ActionResult Create(int id=0)
        {
            
            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "资料员").AsSelectItemList(c => c.Id, c => c.Name);
            if (id > 0) {
                return View(DbHelper.Get<Project>(id));
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Project entity, int documenterId)
        {
            if (entity.Id > 0)
            {
                var ent = DbHelper.Get<Project>(entity.Id);
                ent.Account = entity.Account;
                ent.Bank = entity.Bank;
                ent.CompanyName = entity.CompanyName;
                ent.Documenter= DbHelper.Get<Manager>(documenterId);
                ent.EndDate = entity.EndDate;
                ent.Link = entity.Link;
                ent.MaterialFee = entity.MaterialFee;
                ent.Money = entity.Money;
                ent.OpenDate = entity.OpenDate;
                ent.OpenPeople = entity.OpenPeople;
                ent.Owner = entity.Owner;
                ent.ProjectName = entity.ProjectName;
                ent.Scale = entity.Scale;
                ent.Situation = entity.Situation;
                ent.Source = entity.Source;
                ent.Type = entity.Type;
                ent.UserName = entity.UserName;
                ent.SalesOpinion = entity.SalesOpinion;
                ent.Update(true);
            }
            else {
                entity.ProjectProgress = "已上网";
                entity.Sale = ManagerImp.Current;
                entity.CreateDate = DateTime.Now;
                entity.Documenter = DbHelper.Get<Manager>(documenterId);
                entity.Save(true);
            }
           
            return Util.Echo(100, "操作成功", "保存项目成功", "/Project/Index");
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Project>(id));
        }

        [HttpGet]
        public ActionResult Audit(int id)
        {
            var entity = DbHelper.Get<Project>(id);
            if (entity.ProjectProgress == "已上网")
            {
                ViewBag.Next = "财务";
                ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "财务").AsSelectItemList(c => c.Id, c => c.Name);
            }
            else if (entity.ProjectProgress == "做资料")
            {
                ViewBag.Next = "总经理";
                ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "总经理").AsSelectItemList(c => c.Id, c => c.Name);
            }
            else if (entity.ProjectProgress == "打保证金")
            {
                ViewBag.Next = "开标人";
                ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "开标人").AsSelectItemList(c => c.Id, c => c.Name);
            }
           
            return View(entity);
        }

        [HttpPost]
        public ActionResult Audit(int id, string opinion, bool agree, int managerId=0)
        {
            var entity = DbHelper.Get<Project>(id);
            if (entity.ProjectProgress == "已上网")
            {
                entity.DocumenterOpinion = opinion;
                if (agree)
                {
                    entity.Finance = DbHelper.Get<Manager>(managerId);
                    entity.ProjectProgress = "做资料";
                }
            }
            else if (entity.ProjectProgress == "做资料")
            {
                entity.FinanceOpinion = opinion;
                if (agree)
                {
                    entity.CEO = DbHelper.Get<Manager>(managerId);
                    entity.ProjectProgress = "打保证金";
                }
            }
            else if (entity.ProjectProgress == "打保证金")
            {
                entity.CEOOpinion = opinion;
                if (agree)
                {
                    entity.OpenPeople = DbHelper.Get<Manager>(managerId);
                    entity.ProjectProgress = "开标结束";
                }
            }
            else if (entity.ProjectProgress == "开标结束")
            {
                entity.Situation = opinion;
                if (agree)
                {
                    entity.ProjectProgress = "保证金已退";
                }
            }
            if (!agree)
            {
                entity.ProjectProgress = "不能投标";
            }
            entity.Update(true);
            return Util.Echo(100, "操作成功", "审核项目成功", "/Project/Index");
        }

    }
}
