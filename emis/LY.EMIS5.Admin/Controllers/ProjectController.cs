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
using LY.EMIS5.Admin.Models;
using Newtonsoft.Json;

namespace LY.EMIS5.Admin.Controllers
{
    public class ProjectController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            ViewBag.Company= DbHelper.Query < Company >().AsSelectItemList(c => c.Id, c => c.Name);
            ViewBag.ProjectProgresses = EnumHelper<ProjectProgresses>.EnumToSelectList();
            var list = DbHelper.Query<Manager>(c => c.Kind == "业务员").AsSelectItemList(c => c.Id, c => c.Name);
            list.Insert(0, new SelectListItem() { Text = "请选择业务员", Value = "0" });
            ViewBag.Sales = list;
            return View();
        }
        [HttpGet, Authorize]
        public ActionResult SaleIndex()
        {
            ViewBag.Company = DbHelper.Query<Company>().AsSelectItemList(c => c.Id, c => c.Name);
            ViewBag.ProjectProgresses = EnumHelper<ProjectProgresses>.EnumToSelectList();
            var list = DbHelper.Query<Manager>(c => c.Kind == "业务员").AsSelectItemList(c => c.Id, c => c.Name);
            list.Insert(0, new SelectListItem() { Text = "请选择业务员", Value = "0" });
            ViewBag.Sales = list;
            return View();
        }

        [HttpPost, Authorize]
        public string Index(int iDisplayStart = 0, int iDisplayLength = 15, string name = "", string company="", string begin="", string end = "", int sale = 0, int iSortCol_0=6,string sSortDir_0 = "desc",  int state =0, string sEcho = "")
        {
            IQueryable<Project> query = DbHelper.Query<Project>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.ProjectName.Contains(name));
            }
            if (state>0)
            {
                query = query.Where(c => c.ProjectProgress == (ProjectProgresses)state);
            }
            if (sale > 0)
            {
                query = query.Where(c => c.Sale.Id == sale);
            }
            if (!string.IsNullOrEmpty(company)) {
                query = query.Where(c => c.CompanyName.Contains(company));
            }
            if (!string.IsNullOrEmpty(begin))
            {
                query = query.Where(c => c.OpenDate>=DateTime.Parse(begin));
            }
            if (!string.IsNullOrEmpty(end))
            {
                query = query.Where(c => c.OpenDate <= DateTime.Parse(end).AddDays(1));
            }

            if (iSortCol_0 == 6)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Sort).OrderByDescending(c => c.Id);
                }
                else {
                    query = query.OrderBy(c => c.Sort).OrderByDescending(c => c.Id);
                }
            }
            else if (iSortCol_0 == 0)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Id);
                }
                else {
                    query = query.OrderBy(c => c.Id);
                }
            }
            else if (iSortCol_0 == 7)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Sort).OrderByDescending(c => c.OpenDate);
                }
                else {
                    query = query.OrderByDescending(c => c.Sort).OrderBy(c => c.OpenDate);
                }
            }
            else if (iSortCol_0 == 8)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Sort).OrderByDescending(c => c.EndDate);
                }
                else {
                    query = query.OrderByDescending(c => c.Sort).OrderBy(c => c.EndDate);
                }
            }
            else if (iSortCol_0 == 10)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.CreateDate);
                }
                else {
                    query = query.OrderBy(c => c.CreateDate);
                }
            }
            else {
                query = query.OrderByDescending(c => c.Sort).OrderBy(c => c.ProjectProgress);
            }
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    ProjectName = c.ProjectName,
                    Name = c.Sale.Name,
                    c.Scale,
                    c.Money,
                    c.Source,
                    ProjectProgress=c.ProjectProgress.GetDescription(),
                    OpenDate =c.OpenDate.Year<1000?"": c.OpenDate.ToChineseDateString(),
                    EndDate = c.EndDate.Year < 1000 ? "": c.EndDate.ToChineseDateString(),
                    CreateDate = c.CreateDate.ToYearMonthDayString(),
                    c.CompanyName,
                    Edit=ManagerImp.Current.Kind=="管理员"|| ManagerImp.Current.Kind=="总经理",
                    //Prompt = c.ProjectProgress != "未上网" && (c.OpenDate - DateTime.Now).Days < 30
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult AuditList()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string AuditList(int iDisplayStart = 0, int iDisplayLength = 15, string name = "", int state = 0, string sEcho = "", int iSortCol_0 = 7, string sSortDir_0 = "desc")
        {
            IQueryable<Project> query = DbHelper.Query<Project>(c => c.Sale.Id == ManagerImp.Current.Id || c.Current.Manager.Id==ManagerImp.Current.Id && !c.Current.Done);
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.ProjectName.Contains(name));
            }
            if (state>0)
            {
                query = query.Where(c => c.ProjectProgress ==  (ProjectProgresses)state);
            }
            if (iSortCol_0 == 6)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Sort).OrderByDescending(c => c.Id);
                }
                else {
                    query = query.OrderBy(c => c.Sort).OrderByDescending(c => c.Id);
                }
            }
            else if (iSortCol_0 == 0)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Id);
                }
                else {
                    query = query.OrderBy(c => c.Id);
                }
            }
            else if (iSortCol_0 == 7)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Sort).OrderByDescending(c => c.OpenDate);
                }
                else {
                    query = query.OrderByDescending(c => c.Sort).OrderBy(c => c.OpenDate);
                }
            }
            else if (iSortCol_0 == 8)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.Sort).OrderByDescending(c => c.EndDate);
                }
                else {
                    query = query.OrderByDescending(c => c.Sort).OrderBy(c => c.EndDate);
                }
            }
            else if (iSortCol_0 == 10)
            {
                if (sSortDir_0 == "desc")
                {
                    query = query.OrderByDescending(c => c.CreateDate);
                }
                else {
                    query = query.OrderBy(c => c.CreateDate);
                }
            }
            else {
                query = query.OrderByDescending(c => c.Sort).OrderBy(c => c.ProjectProgress);
            }
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    ProjectName = c.ProjectName,
                    Name = c.Sale.Name,
                    c.Scale,
                    c.Money,
                    c.Source,
                    ProjectProgress = c.ProjectProgress.GetDescription(),
                    OpenDate = c.OpenDate.Year < 1000 ? "" : c.OpenDate.ToChineseDateString(),
                    EndDate = c.EndDate.Year < 1000 ? "" : c.EndDate.ToChineseDateString(),
                    c.CompanyName,
                    CreateDate = c.CreateDate.ToYearMonthDayString(),
                    Edit = ManagerImp.Current.Id == c.Sale.Id && c.ProjectProgress != ProjectProgresses.Success && c.ProjectProgress != ProjectProgresses.Cancel,
                    Open=c.ProjectProgress==ProjectProgresses.Arrange,
                    Revoke = c.ProjectProgress != ProjectProgresses.NotOnline && c.ProjectProgress!= ProjectProgresses.Cancel && c.ProjectProgress != ProjectProgresses.Success && ManagerImp.Current.Id == c.Sale.Id,
                    Audit = c.ProjectProgress != ProjectProgresses.NotOnline && c.Opinions.Any(m => m.Manager.Id == ManagerImp.Current.Id && !m.Done),
                    Prompt = c.ProjectProgress != ProjectProgresses.Success && c.ProjectProgress!=ProjectProgresses.Cancel && (c.OpenDate - DateTime.Now).Days < 30
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id = 0)
        {
            var flow = HttpRuntime.Cache.Get("flow") as Flow;
            flow.setAction();
            ViewBag.Flow = flow;
            ViewBag.Companys = DbHelper.Query<Company>().ToList();
            var sales = DbHelper.Query<Manager>(c => c.Kind == flow.src.role).AsSelectItemList(c => c.Id, c => c.Name);
            var sale = sales.FirstOrDefault(c => c.Value == ManagerImp.Current.Id.ToString());
            
            ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == flow.dest.role).AsSelectItemList(c => c.Id, c => c.Name);
            var Project = new Project { ProjectProgress = ProjectProgresses.NotOnline };
            if (id > 0)
            {
                Project = DbHelper.Get<Project>(id);
                sale = sales.FirstOrDefault(c => c.Value == Project.Sale.Id.ToString());
            }
            if (sale!=null)
            {
                sale.Selected = true;
            }
            ViewBag.Sales = sales;
            return View(Project);
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
                entity.CompanyName = Request.Form["companys"];
                entity.ReplaceMoney = model.ReplaceMoney;
                entity.Aptitude = model.Aptitude;
                entity.Remark = model.Remark;
                entity.MoneySituation = model.MoneySituation;
                entity.Sale = model.Sale;
                entity.Proxy = model.Proxy;
                entity.Requirement = model.Requirement;
                if (entity.ProjectProgress != ProjectProgresses.NotOnline) {
                    entity.Sort = 1;
                }
                Flow flow;
                if (model.Id > 0)
                {
                    flow = JsonConvert.DeserializeObject<Flow>(entity.Flow);
                    entity.Update();
                }
                else
                {
                    entity.Flow = HttpRuntime.Cache.Get("json").ToString();
                    flow = JsonConvert.DeserializeObject<Flow>(entity.Flow);
                    entity.CreateDate = DateTime.Now;
                    entity.Save();
                    var Progress = entity.ProjectProgress.GetDescription();
                    entity.Current=new Opinion { Content = entity.SalesOpinion, Agree = false, CreateDate = DateTime.Now, Done = false, Manager = entity.Sale, Project = entity, ProjectProgress = Progress, NodeId=flow.nodes.First(c=>c.name== Progress).id }.Save();
                    entity.Update();
                }
                flow = JsonConvert.DeserializeObject<Flow>(entity.Flow);
                if (entity.ProjectProgress != ProjectProgresses.NotOnline)
                {
                    var Progress = entity.ProjectProgress.GetDescription();
                    var op= new Opinion { Agree = false, CreateDate = DateTime.Now, Done = false, Manager = DbHelper.Get<Manager>(documentId), Project = entity, ProjectProgress = Progress, NodeId = flow.nodes.First(c => c.name == Progress).id, Src= entity.Current }.Save();
                    entity.Current.Dest = op;
                    entity.Current.Agree = true;
                    entity.Current.DoneDate = DateTime.Now;
                    entity.Current.Done = true;
                    entity.Current.Update();
                    entity.Current = op;
                    entity.ProjectProgress = EnumHelper<ProjectProgresses>.EnumFromString(op.ProjectProgress);
                    entity.Update();

                }
                ts.Complete();
            }
           
            return this.RedirectToAction(100, "操作成功", "保存项目成功", "Project", "AuditList");
        }

        [HttpGet, Authorize]
        public ActionResult Edit(int id)
        {
            var pro = DbHelper.Get<Project>(id);
            var list = DbHelper.Query<Manager>(c=>c.Kind!="管理员").AsSelectItemList(c => c.Id, c => c.Name);
            list.Insert(0, new SelectListItem { Text = "", Value = "0" });
            if (pro.OpenManager != null) {
                list.First(c => c.Value == pro.OpenManager.Id.ToString()).Selected = true;
            }
            ViewBag.List = list;
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
                    entity.OpenDate = model.OpenDate;
                    entity.EndDate = model.EndDate;            
                entity.Update();
                
                ts.Complete();
            }

            return this.RedirectToAction(100, "操作成功", "修改项目成功", "Project", "AuditList");
        }

        [HttpGet, Authorize]
        public ActionResult SetOpen(int id)
        {
            var pro = DbHelper.Get<Project>(id);
            var list = DbHelper.Query<Manager>(c => c.Kind != "管理员").AsSelectItemList(c => c.Id, c => c.Name);
            if (pro.OpenManager != null)
            {
                list.First(c => c.Value == pro.OpenManager.Id.ToString()).Selected = true;
            }
            ViewBag.List = list;
            return View(DbHelper.Get<Project>(id));

        }

        [HttpPost, Authorize]
        public ActionResult SetOpen(Project model,int openId)
        {
            using (var ts = TransactionScopes.Default)
            {
                var entity = DbHelper.Get<Project>(model.Id);
                entity.OpenManager = DbHelper.Get<Manager>(openId);
                entity.Update();

                ts.Complete();
            }

            return this.RedirectToAction(100, "操作成功", "设置开标人成功", "Project", "AuditList");
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
            var flow = JsonConvert.DeserializeObject<Flow>(entity.Flow);
            flow.setAction(flow.actions.First(c => c.src == entity.Current.NodeId));
            ViewBag.Flow = flow;
            if(flow.dest.role!=null)
                ViewBag.List = DbHelper.Query<Manager>(c => c.Kind == flow.dest.role).AsSelectItemList(c => c.Id, c => c.Name);
            return View(entity);
        }

        [HttpPost, Authorize]
        public ActionResult Audit(int id, string content, int managerId = 0)
        {
            using (var ts = TransactionScopes.Default)
            {

                var entity = DbHelper.Get<Project>(id);
                var flow = JsonConvert.DeserializeObject<Flow>(entity.Flow);
                flow.setAction(flow.actions.First(c => c.src == entity.Current.NodeId));
                entity.Current.Content = content;
                entity.Current.DoneDate = DateTime.Now;
                entity.Current.Done = true;
                entity.Current.Update();
                if (flow.dest.person != null) {
                    if (flow.dest.person == "创建人")
                        managerId = entity.Sale.Id;
                }
                if (managerId != 0)
                {
                    entity.Current = new Opinion { Agree = false, CreateDate = DateTime.Now, Done = false, Manager = DbHelper.Get<Manager>(managerId), Project = entity, ProjectProgress = flow.dest.name, NodeId = flow.dest.id, Src = entity.Current }.Save();
                }
                else {
                    entity.Current = null;
                }
                entity.ProjectProgress = EnumHelper<ProjectProgresses>.EnumFromString(flow.dest.name);
                entity.Update();
                ts.Complete();
            }
            return this.RedirectToAction(100, "操作成功", "审核项目成功", "Project", "AuditList");
        }


        [HttpGet, Authorize]
        public ActionResult Cancel(int id)
        {
            var entity = DbHelper.Get<Project>(id);
            entity.Current = null;
            entity.ProjectProgress = ProjectProgresses.Cancel;
            entity.Update(true);
            return this.RedirectToAction(100, "操作成功", "项目删除成功", "Project", "AuditList");
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
                entity.ProjectProgress = ProjectProgresses.Cancel;
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
                entity.Delete(true);
            return this.RedirectToAction(100, "操作成功", "项目删除成功", "Project", "Index");
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
            return Json( DbHelper.Query<Manager>(c => c.Kind == opinion.Kind).Select(c => new{ c.Id, c.Name}), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManagerByKind(string company,string kind)
        {
            return Json(DbHelper.Query<Manager>(c => c.Kind == kind).Select(c => new { c.Id, c.Name }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Authorize]
        public ActionResult Open(int id)
        {
            return View(DbHelper.Get<Project>(id));
        }

        [HttpPost, Authorize]
        public ActionResult Open(Project model)
        {
            var entity = DbHelper.Get<Project>(model.Id);
            entity.IsOpen = true;
            entity.OpenRemark = model.OpenRemark;
            entity.Bid = model.Bid;
            entity.Update(true);
            return this.RedirectToAction(100, "操作成功", "项目开标完成", "Home", "Index");
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
                    //list.Add(new { Name = c.Name, Register = query.Where(m => m.CompanyName == item.Name).Count(), NotOnLine = query.Where(m => m.CompanyName == item.Name && m.ProjectProgress == "未上网").Count(), Cannot = query.Where(m => m.CompanyName == item.Name && m.ProjectProgress == "不能投标").Count(), Open = query.Where(m => m.CompanyName == item.Name && (m.ProjectProgress== "开标结束" || m.ProjectProgress == "项目结束" || m.ProjectProgress == "保证金已退") ).Count(), Company = item.Name });
                }
               
            });

            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                list.Count(),
                list.ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult Export()
        {
            
            var Table = new DataTable("项目表");
            Table.Columns.Add("编号").ReadOnly = true;
            Table.Columns.Add("项目名称").ReadOnly = true;
            Table.Columns.Add("进度").ReadOnly = true;
            Table.Columns.Add("业务员").ReadOnly = true;
            Table.Columns.Add("创建时间").ReadOnly = true;
            Table.Columns.Add("下载链接").ReadOnly = true;
            Table.Columns.Add("规模").ReadOnly = true;
            Table.Columns.Add("业主").ReadOnly = true;
            Table.Columns.Add("保证金来源").ReadOnly = true;
            Table.Columns.Add("代打金额").ReadOnly = true;
            Table.Columns.Add("保证金金额(万元)").ReadOnly = true;
            Table.Columns.Add("保证金截至时间").ReadOnly = true;
            Table.Columns.Add("保证金打款方式").ReadOnly = true;
            Table.Columns.Add("保证金账户名").ReadOnly = true;
            Table.Columns.Add("保证金账号").ReadOnly = true;
            Table.Columns.Add("保证金开户行").ReadOnly = true;
            Table.Columns.Add("开标时间").ReadOnly = true;
            Table.Columns.Add("开标人").ReadOnly = true;
            Table.Columns.Add("是否中标").ReadOnly = true;
            Table.Columns.Add("资料费(元)").ReadOnly = true;
            Table.Columns.Add("公司名称").ReadOnly = true;
            Table.Columns.Add("资质要求").ReadOnly = true;
            Table.Columns.Add("收费情况").ReadOnly = true;
            Table.Columns.Add("代理公司").ReadOnly = true;
            Table.Columns.Add("人员要求").ReadOnly = true;
            foreach (var item in DbHelper.Query<Project>().OrderByDescending(c => c.Id))
            {
                var Row = Table.NewRow();
                Row["编号"] = item.Id;
                Row["项目名称"] = item.ProjectName;
                Row["进度"] = item.ProjectProgress.GetDescription();
                Row["业务员"] = item.Sale.Name;
                Row["创建时间"] = item.CreateDate.ToChineseDateString();
                Row["下载链接"] = item.Link;
                Row["规模"] = item.Scale;
                Row["业主"] = item.Owner;
                Row["保证金来源"] = item.Source;
                Row["代打金额"] = item.ReplaceMoney;
                Row["保证金金额(万元)"] = item.Money;
                Row["保证金截至时间"] = item.EndDate;
                Row["保证金打款方式"] = item.Type;
                Row["保证金账户名"] = item.UserName;
                Row["保证金账号"] = item.Account;
                Row["保证金开户行"] = item.Bank;
                Row["开标时间"] = item.OpenDate;
                Row["开标人"] = item.OpenManager==null?"":item.OpenManager.Name;
                Row["是否中标"] = item.Bid?"是":"否";
                Row["资料费(元)"] = item.MaterialFee;
                Row["公司名称"] = item.CompanyName;
                Row["资质要求"] = item.Aptitude;
                Row["收费情况"] = item.MoneySituation;
                Row["代理公司"] = item.Proxy;
                Row["人员要求"] = item.Requirement;
                Table.Rows.Add(Row);
            }
            return Table.ExportToXls(Table.TableName + ".xls");
        }
    }

}
