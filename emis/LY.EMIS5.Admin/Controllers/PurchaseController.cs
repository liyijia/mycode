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
    public class PurchaseController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Purchase> query = DbHelper.Query<Purchase>();
            if (ManagerImp.Current.Kind == "项目经理")
            {
                query = query.Where(m => m.Manager.Id == ManagerImp.Current.Id && m.Status == 0);
            }
            else if (ManagerImp.Current.Kind == "材料员")
            {
                query = query.Where(m => m.Status == 1 || (m.Buyer.Id == ManagerImp.Current.Id && m.Status == 2));
            }
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
            IQueryable<Purchase> query = DbHelper.Query<Purchase>().Where(m => m.Creator.Id == ManagerImp.Current.Id || m.Manager.Id == ManagerImp.Current.Id || m.Buyer.Id == ManagerImp.Current.Id);
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new { c.Id, c.Dictionary.Name, Creator = c.Creator.Name, CreateDate = c.CreateDate.ToChineseDateString(), c.Status }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            ViewBag.List = DbHelper.Query<Dictionary>().AsSelectItemList(c => c.Id, c => c.Name);
            ViewBag.Materials = DbHelper.Query<Material>().OrderBy(c=>c.Type).ToList();
            if (id > 0) {
                return View(DbHelper.Get<Purchase>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(Purchase entity)
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
                    entity.Manager = entity.Dictionary.Manager;
                    entity.Save();
                    entity.Materials.ToList().ForEach(c => {
                        c.PurchaseId = entity.Id;
                        c.Save();
                    });
                    ts.Complete();
                }
            }
            return this.RedirectToAction(100, "操作成功", "申请提交成功!", "Purchase", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Audit(int id = 0)
        {
            if (id > 0)
            {
                return View(DbHelper.Get<Purchase>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Audit(Purchase entity)
        {
            var old = DbHelper.Get<Purchase>(entity.Id);
            
            old.Status = entity.Status;
            if (old.Status == 1)
            {
                old.ManagerDate = DateTime.Now;
                old.ManagerContent = entity.ManagerContent;
            }
            else if (old.Status == 2)
            {
                old.AcceptDate = DateTime.Now;
                old.Buyer = ManagerImp.Current;
            }
            else if (old.Status == 3)
            {
                old.BuyerDate = DateTime.Now;
                old.BuyerContent = entity.BuyerContent;
            }
            old.Update(true);
            if (old.Status == 3) {
                return this.RedirectToAction(100, "操作成功", "处理申请成功,即将进行入库操作!", "Storage", "Create",new { PurchaseId=entity.Id });
            }
            return this.RedirectToAction(100, "操作成功", "处理申请成功!", "Purchase", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Purchase>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除项目成功!", "Purchase", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Purchase>(id));
        }

    }
}
