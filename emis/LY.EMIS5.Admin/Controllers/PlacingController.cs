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
    public class PlacingController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Placing> query = DbHelper.Query<Placing>(m => m.Auditor.Id == ManagerImp.Current.Id && m.Status == 0);
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new { c.Id,c.Dictionary.Name, Creator=c.Creator.Name, CreateDate=c.CreateDate.ToChineseDateString(),c.Status }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult History()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string History(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Placing> query = DbHelper.Query<Placing>().Where(m => m.Creator.Id == ManagerImp.Current.Id || m.Auditor.Id == ManagerImp.Current.Id || m.Receiptor.Id == ManagerImp.Current.Id);
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new { c.Id, c.Dictionary.Name, Creator = c.Creator.Name, CreateDate = c.CreateDate.ToChineseDateString(), c.Status }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            ViewBag.List = DbHelper.Query<Dictionary>().AsSelectItemList(c => c.Id, c => c.Name);
            ViewBag.Managers=DbHelper.Query<Manager>(c=>c.Kind== "施工员").AsSelectItemList(c => c.Id, c => c.Name);
            ViewBag.Materials = DbHelper.Query<Material>().OrderBy(c=>c.Type).ToList();
            if (id > 0) {
                return View(DbHelper.Get<Placing>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(Placing entity)
        {
            if (entity.Id > 0)
            {
                entity.Update(true);
            }
            else {
                using (var ts = TransactionScopes.Default)
                {
                    entity.Dictionary = DbHelper.Get<Dictionary>(entity.Dictionary.Id);
                    entity.CreateDate = DateTime.Now;
                    entity.Creator = ManagerImp.Current;
                    entity.Auditor = entity.Dictionary.Manager;
                    entity.No = DateTime.Now.ToString("yyyyMMddHHmmss");
                    entity.Save();
                    entity.Details.ToList().ForEach(c => {
                        c.PlacingId = entity.Id;
                        c.Save();
                    });
                    ts.Complete();
                }
            }
            return this.RedirectToAction(100, "操作成功", "申请提交成功!", "Placing", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Audit(int id = 0)
        {
            if (id > 0)
            {
                return View(DbHelper.Get<Placing>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Audit(Placing entity)
        {
            var old = DbHelper.Get<Placing>(entity.Id);
            
            old.Status = entity.Status;
            old.AuditDate = DateTime.Now;
            using (var ts = TransactionScopes.Default)
            {
                old.Update();
                if (old.Status == 1) {
                    old.Details.ToList().ForEach(c =>
                    {
                        if (c.Material.Stock < c.Number)
                        {
                            throw new EmisException(200, "操作失败", "材料" + c.Material.Name + "库存不足");
                        }
                        else {
                            c.Material.Stock -= c.Number;
                            c.Update();
                            DbHelper.Query<Goods>(m => m.Status == 0).OrderBy(m => m.InDate).Take(c.Number).ToList().ForEach(m => {
                                m.OutDate = DateTime.Now;
                                m.Placing = old;
                                m.Status = 1;
                                m.Update();
                            });
                        }
                    });
                }
                ts.Complete();
            }
            return this.RedirectToAction(100, "操作成功", "处理申请成功!", "Placing", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Placing>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除项目成功!", "Placing", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Placing>(id));
        }

        [HttpGet, Authorize]
        public ActionResult Report(string begin="",string end="")
        {
            var query = DbHelper.Query<Goods>(c => c.Status == 1);
            if (!string.IsNullOrEmpty(begin)) {
                query = query.Where(c => c.OutDate >= DateTime.Parse(begin));
            }
            if (!string.IsNullOrEmpty(end)) {
                query = query.Where(c => c.OutDate <= DateTime.Parse(end+" 23:59:59"));
            }
            return View(query.GroupBy(c => new { c.Placing.Dictionary, c.Material }).ToList().OrderBy(c => c.Key.Dictionary).OrderBy(c => c.Key.Material).Select(g => Tuple.Create(g.Key.Dictionary.Name, g.Key.Material.Name, g.Count(), g.Sum(m => m.Price))).ToList());
        }

    }
}
