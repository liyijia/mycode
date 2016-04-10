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
using LY.EMIS5.Common.Mvc.Extensions;

namespace LY.EMIS5.Admin.Controllers
{
    public class ProjectController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            var list = DbHelper.Query<Manager>(c => c.Kind == "业务员").AsSelectItemList(c => c.Id, c => c.Name);
            list.Insert(0, new SelectListItem() { Text = "请选择业务员", Value = "0" });
            ViewBag.Sales = list;
            return View();
        }

        [HttpPost, Authorize]
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
                query.Skip(iDisplayStart).Take(iDisplayLength).OrderByDescending(c => c.Sort).OrderByDescending(c => c.Id).ToList().Select(c => new
                {
                    Id = c.Id,
                    ProjectName = c.ProjectName,
                    Name = c.Sale.Name,
                    c.Scale,
                    c.Money,
                    c.Source,
                    c.ProjectProgress,
                    OpenDate =c.OpenDate.Year<1000?"": c.OpenDate.ToChineseDateString(),
                    EndDate = c.EndDate.Year < 1000 ? "": c.EndDate.ToChineseDateString(),
                    CreateDate = c.CreateDate.ToYearMonthDayString(),
                    c.CompanyName,
                    Edit=ManagerImp.Current.Kind=="管理员"|| ManagerImp.Current.Kind=="资料员",
                    Prompt = c.ProjectProgress != "未上网" && (c.OpenDate - DateTime.Now).Days < 30
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult AuditList()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string AuditList(int iDisplayStart = 0, int iDisplayLength = 15, string name = "", string state = "", string sEcho = "")
        {
            IQueryable<Project> query = DbHelper.Query<Project>(c => c.Sale.Id == ManagerImp.Current.Id || c.Opinions.Any(m => m.Manager.Id == ManagerImp.Current.Id && !m.Done));
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.ProjectName.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                query = query.Where(c => c.ProjectProgress == state);
            }
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).OrderByDescending(c => c.CreateDate).ToList().Select(c => new
                {
                    Id = c.Id,
                    ProjectName = c.ProjectName,
                    Name = c.Sale.Name,
                    c.Scale,
                    c.Money,
                    c.Source,
                    c.ProjectProgress,
                    OpenDate = c.OpenDate.Year < 1000 ? "" : c.OpenDate.ToChineseDateString(),
                    EndDate = c.EndDate.Year < 1000 ? "" : c.EndDate.ToChineseDateString(),
                    c.CompanyName,
                    CreateDate = c.CreateDate.ToYearMonthDayString(),
                    Edit = ManagerImp.Current.Id == c.Sale.Id,
                    Revoke = c.ProjectProgress != "未上网" && c.ProjectProgress!= "作废审核" && ManagerImp.Current.Id == c.Sale.Id,
                    Audit = c.Opinions.Any(m => m.Manager.Id == ManagerImp.Current.Id && !m.Done),
                    Prompt = c.ProjectProgress != "未上网" && (c.OpenDate - DateTime.Now).Days < 30
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id = 0)
        {
            ViewBag.Companys = DbHelper.Query<Company>().ToList();
            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "资料员").AsSelectItemList(c => c.Id, c => c.Name);
            if (id > 0)
            {
                return View(DbHelper.Get<Project>(id));
            }
            return View(new Project { ProjectProgress="未上网"});
        }

        [HttpPost, Authorize]
        public ActionResult Create(Project model, int documentId = 0)
        {
            var entity = new Project();
            using (var ts = TransactionScopes.Default)
            {
                if (model.Id > 0)
                {
                    entity = DbHelper.Get<Project>(model.Id);
                }
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
                entity.ProjectProgress = model.ProjectProgress;
                entity.Account = model.Account;
                entity.Bank = model.Bank;
                entity.CompanyName = model.CompanyName;
                entity.ReplaceMoney = model.ReplaceMoney;
                entity.Aptitude = model.Aptitude;
                entity.Remark = model.Remark;
                entity.MoneySituation = model.MoneySituation;
                if (entity.ProjectProgress != "未上网") {
                    entity.Sort = 1;
                }
                if (model.Id > 0)
                {
                    entity.Update();
                }
                else
                {
                    entity.Sale = ManagerImp.Current;
                    entity.CreateDate = DateTime.Now;
                    entity.Save();
                }
                if (entity.ProjectProgress != "未上网")
                {
                    new Opinion { Agree = false, CreateDate = DateTime.Now, Done = false, Manager = DbHelper.Get<Manager>(documentId), Project = entity, ProjectProgress = "做资料", Kind = "财务" }.Save();
                }
                ts.Complete();
            }
           
            return this.RedirectToAction(100, "操作成功", "保存项目成功", "Project", "AuditList");
        }

        [HttpGet, Authorize]
        public ActionResult Edit(int id)
        {
           
                return View(DbHelper.Get<Project>(id));
          
        }

        [HttpPost, Authorize]
        public ActionResult Edit(Project model)
        {
            using (var ts = TransactionScopes.Default)
            {
                    var entity = DbHelper.Get<Project>(model.Id);
                entity.MaterialFee = model.MaterialFee;
                entity.Source = model.Source;
                entity.ReplaceMoney = model.ReplaceMoney;
                entity.Remark = model.Remark;
                
                    entity.Update();
                
                ts.Complete();
            }

            return this.RedirectToAction(100, "操作成功", "修改项目成功", "Project", "AuditList");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Project>(id));
        }

        [HttpGet, Authorize]
        public ActionResult Audit(int id)
        {
            var entity = DbHelper.Get<Project>(id);
            var opinion = entity.Opinions.FirstOrDefault(c => !c.Done);
            if(opinion!=null && opinion.Kind!="")
                ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == opinion.Kind).AsSelectItemList(c => c.Id, c => c.Name);
            return View(entity);
        }

        [HttpPost, Authorize]
        public ActionResult Audit(int opinionid, string content, bool agree, int managerId = 0)
        {
            using (var ts = TransactionScopes.Default)
            {
                var entity = DbHelper.Get<Opinion>(opinionid);
                if (Request.Form["bid"] != null) {
                    entity.Project.Bid = bool.Parse( Request.Form["bid"]);
                }
                entity.Content = content;
                if (agree)
                {
                    entity.Project.ProjectProgress = entity.ProjectProgress;
                    if (managerId > 0)
                    {
                        Opinion op = new Opinion { Agree = false, CreateDate = DateTime.Now, Done = false, Manager = DbHelper.Get<Manager>(managerId), Project = entity.Project };
                        if (entity.ProjectProgress == "做资料")
                        {
                            op.ProjectProgress = "打保证金";
                            op.Kind = "总经理";
                        }
                        else if (entity.ProjectProgress == "打保证金")
                        {
                            op.ProjectProgress = "同意开标";
                            op.Kind = "总经理";
                        }
                        else if (entity.ProjectProgress == "同意开标")
                        {
                            op.ProjectProgress = "开标结束";
                            op.Kind = "财务";
                        }
                        else if (entity.ProjectProgress == "开标结束")
                        {
                            op = null;
                        }
                        if(op!=null)
                            op.Save();
                    }
                }
                else
                {
                    entity.Project.Sort = -1;
                    entity.Project.ProjectProgress = "不能投标";
                }
                entity.Done = true;
                entity.Agree = agree;
                entity.DoneDate = DateTime.Now;
                entity.Update();
                entity.Project.Update();
                ts.Complete();
            }
            return this.RedirectToAction(100, "操作成功", "审核项目成功", "Project", "AuditList");
        }


        [HttpGet, Authorize]
        public ActionResult Cancel(int id)
        {
            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == "总经理").AsSelectItemList(c => c.Id, c => c.Name);
            return View(DbHelper.Get<Project>(id));
        }

        [HttpPost, Authorize]
        public ActionResult Cancel(int id,string content,int managerId)
        {
            using (var ts = TransactionScopes.Default)
            {
                var entity = DbHelper.Get<Project>(id);
                entity.Opinions.Where(c => !c.Done).ForEach(c =>
                {
                    c.Delete();
                });
                entity.Sort = -1;
                entity.ProjectProgress = "作废审核";
                entity.Update();
                new Opinion { Agree=true, Content=content, CreateDate=DateTime.Now,Done=true, DoneDate= DateTime.Now, Kind= "总经理", Manager=ManagerImp.Current, Project= entity, ProjectProgress="作废申请" }.Save();
                new Opinion { CreateDate = DateTime.Now, Kind = "", Manager = DbHelper.Get<Manager>(managerId), Project = entity, ProjectProgress = "作废审核" }.Save();
                ts.Complete();
            }
            return this.RedirectToAction(100, "操作成功", "项目作废申请成功", "Project", "AuditList");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id)
        {
            var entity = DbHelper.Get<Project>(id);
            if (entity.ProjectProgress == "未上网") {
                entity.Delete(true);
            }
            return this.RedirectToAction(100, "操作成功", "项目作废申请成功", "Project", "AuditList");
        }

        public ActionResult SearchProject(string term)
        {
            return Json(DbHelper.Query<Project>(c => c.ProjectName.Contains(term)).Select(c => c.ProjectName), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Top()
        {
            return Json(DbHelper.Query<Project>(c => c.Sale.Id == ManagerImp.Current.Id || c.Opinions.Any(m => m.Manager.Id == ManagerImp.Current.Id && !m.Done)).Select(c => new {c.Id,c.ProjectName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManagerById(int id) {
            var entity = DbHelper.Get<Project>(id);
            var opinion = entity.Opinions.FirstOrDefault(c => !c.Done);
            return Json( DbHelper.Query<Manager>(c => c.Kind == opinion.Kind && c.Company.Name==entity.CompanyName).Select(c => new{ c.Id, c.Name}), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManagerByKind(string company,string kind)
        {
            return Json(DbHelper.Query<Manager>(c => c.Kind == kind && c.Company.Name == company).Select(c => new { c.Id, c.Name }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Authorize]
        public ActionResult Report()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Report(int iDisplayStart = 0, int iDisplayLength = 25, string beginDate = "", string endDate = "", string sEcho = "")
        {
            List<object> list = new List<object>();
            var companys = DbHelper.Query<Company>().ToList();
            DbHelper.Query<Manager>(c => c.Kind == "业务员").ForEach(c =>
            {
                IQueryable<Project> query = DbHelper.Query<Project>(m => m.Sale.Id == c.Id);
                if (!string.IsNullOrEmpty(beginDate))
                {
                    query = query.Where(m => m.CreateDate >= DateTime.Parse(beginDate));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    query = query.Where(m => m.CreateDate <= DateTime.Parse(endDate).AddDays(1));
                }
                foreach (var item in companys)
                {
                    list.Add(new { Name = c.Name, Register = query.Where(m => m.CompanyName == item.Name).Count(), NotOnLine = query.Where(m => m.CompanyName == item.Name && m.ProjectProgress == "未上网").Count(), Cannot = query.Where(m => m.CompanyName == item.Name && m.ProjectProgress == "不能投标").Count(), Open = query.Where(m => m.CompanyName == item.Name && (m.ProjectProgress== "开标结束" || m.ProjectProgress == "项目结束" || m.ProjectProgress == "保证金已退") ).Count(), Company = item.Name });
                }
               
            });

            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                list.Count(),
                list.ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }


    }

}
