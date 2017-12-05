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
    public class ReturnGoodsController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<ReturnGoods> query = DbHelper.Query<ReturnGoods>(m => m.Auditor.Id == ManagerImp.Current.Id && m.Status == 0);
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
            IQueryable<ReturnGoods> query = DbHelper.Query<ReturnGoods>().Where(m => m.Creator.Id == ManagerImp.Current.Id || m.Auditor.Id == ManagerImp.Current.Id || m.ReturnParty.Id == ManagerImp.Current.Id);
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
                return View(DbHelper.Get<ReturnGoods>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(ReturnGoods entity)
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
                        c.ReturnGoodsId = entity.Id;
                        c.Save();
                    });
                    ts.Complete();
                }
            }
            return this.RedirectToAction(100, "操作成功", "申请提交成功!", "ReturnGoods", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Audit(int id = 0)
        {
            if (id > 0)
            {
                return View(DbHelper.Get<ReturnGoods>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Audit(ReturnGoods entity)
        {
            var old = DbHelper.Get<ReturnGoods>(entity.Id);
            
            old.Status = entity.Status;
            old.AuditDate = DateTime.Now;
            var count = 0;
            using (var ts = TransactionScopes.Default)
            {
                if (old.Status == 1) {
                    count = DbHelper.Query<Goods>(m => m.Status == 1 && m.Placing.Dictionary.Id == old.Dictionary.Id).Count();
                    old.Status = 2;
                    if (count > 0) {
                        old.Details.ToList().ForEach(c =>
                        {
                            
                            var list = DbHelper.Query<Goods>(m => m.Status == 1 && m.Placing.Dictionary.Id == old.Dictionary.Id).OrderByDescending(m => m.OutDate).Take(c.Number).ToList();
                            list.ForEach(m => {
                                m.Status = 0;
                                m.Update();
                            });
                            c.Number = list.Count;
                            c.Material.Stock += list.Count;
                            c.Update();
                        });
                    }
                }
                old.Update();
                ts.Complete();
            }
            if (count == 0) {
                return this.RedirectToAction(100, "操作成功", "审批成功，但该项目没有该产品的申请记录!", "ReturnGoods", "Index");
            }
            return this.RedirectToAction(100, "操作成功", "处理申请成功!", "ReturnGoods", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<ReturnGoods>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除项目成功!", "ReturnGoods", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<ReturnGoods>(id));
        }

    }
}
