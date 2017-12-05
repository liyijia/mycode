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
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace LY.EMIS5.Admin.Controllers
{
    public class StorageController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Storage> query = DbHelper.Query<Storage>();
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    c.Id,
                    c.No,
                    c.Total,
                    CreateDate = c.CreateDate.ToChineseDateString(),
                    c.Buyer.Name
                }).ToList<object>())
            { }.ToDataTablesResult(sEcho);
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<News>(id));
        }

        [HttpGet, Authorize]
        public ActionResult Create(int id = 0, string PurchaseId = "")
        {
            ViewBag.Materials = DbHelper.Query<Material>().OrderBy(c => c.Type).ToList();
            ViewBag.Suppliers = JsonConvert.SerializeObject(DbHelper.Query<Supplier>().OrderByDescending(c=>c.Grade).AsSelectItemList(c => c.Id, c => c.Name));
            if (!string.IsNullOrEmpty(PurchaseId))
            {
                ViewBag.PurchaseMaterials = JsonConvert.SerializeObject(DbHelper.Get<Purchase>(PurchaseId).Materials);
            }
            else {
                ViewBag.PurchaseMaterials = "[]";
            }
            if (id > 0)
            {
                return View(DbHelper.Get<Storage>(id));
            }
            return View(new Storage());
        }

        [HttpPost, Authorize]
        [ValidateInput(false)]
        public ActionResult Confirm(List<StorageDetail> StorageDetail)
        {
            var storage = new Storage();
            storage.Buyer = ManagerImp.Current;
            storage.Suppliers = new List<StorageSupplier>();
            StorageDetail.ForEach(c => {
                var supplier = storage.Suppliers.Where(m => m.Supplier.Id == c.StorageSupplier.Supplier.Id).FirstOrDefault();
                if (supplier == null)
                {
                    supplier = c.StorageSupplier;
                    storage.Suppliers.Add(supplier);
                    supplier.Details = new List<StorageDetail>();
                }
                    supplier.Details.Add(c);
            });
            storage.Suppliers.ToList().ForEach(c=> {
                c.Details.ToList().ForEach(m => {
                    c.Total+= m.Number * m.Price;
                });
                storage.Total += c.Total;
            });
            return View(storage);
        }

        [HttpPost, Authorize]
        [ValidateInput(false)]
        public ActionResult Save(Storage storage)
        {
            using (var ts = TransactionScopes.Default)
            {
                storage.Buyer = ManagerImp.Current;
                storage.CreateDate = DateTime.Now;
                storage.No = DateTime.Now.ToString("yyyyMMddHHmmss");
                storage.Save();

                storage.Suppliers.ToList().ForEach(c =>
                {
                    c.Storage = storage;
                    c.Debt = c.Total - c.Payment;
                    c.Save();
                    c.Details.ToList().ForEach(m =>
                    {
                        m.StorageSupplier = c;
                        m.Save();
                        for (var i = 0; i < m.Number; i++)
                        {
                            new Goods() { InDate = DateTime.Now, Material = m.Material, Price = m.Price, Status = 0, Storage = storage }.Save();
                        }
                        var material = DbHelper.Get<Material>(m.Material.Id);
                        material.Stock += m.Number;
                        material.InDate = DateTime.Now;
                        material.Update();
                    });
                });
                ts.Complete();
            }

            return this.RedirectToAction(100, "操作成功", "入库成功!", "Storage", "Index");
        }

    }
}
