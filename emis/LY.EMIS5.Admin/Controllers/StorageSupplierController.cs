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
using Newtonsoft.Json;

namespace LY.EMIS5.Admin.Controllers
{
    public class StorageSupplierController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            ViewBag.Companys = DbHelper.Query<Company>().AsSelectItemList(c => c.Name, c => c.Name);
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string begin = "", string end = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<StorageSupplier> query = DbHelper.Query<StorageSupplier>();
            if (!string.IsNullOrEmpty(begin)) {
                query=query.Where(c => c.Storage.CreateDate >= DateTime.Parse(begin));
            }
            if (!string.IsNullOrEmpty(end)) {
                query = query.Where(c => c.Storage.CreateDate <= DateTime.Parse(end+" 23:59:59"));
            }
            var total = query.Sum(c => c.Total);
            var payment = query.Sum(c => c.Payment);
            var debt = query.Sum(c => c.Debt);
            var result = new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).Select(c => new
                {
                    c.Id,
                    c.Debt,
                    c.IsInvoice,
                    c.Payment,
                    c.Supplier.Name,
                    c.Total,
                    c.Storage.No
                }).ToList<object>())
            { };
            return JsonConvert.SerializeObject(new { sEcho = sEcho, iTotalRecords = result.Total, iTotalDisplayRecords = result.Total, aaData = result.QueryResult,total= total, payment= payment,debt=debt });
        }

        

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            ViewBag.Companys = DbHelper.Query<Company>().ToList();
            if (id > 0) {
                return View(DbHelper.Get<StorageSupplier>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(StorageSupplier entity,decimal pay)
        {
            if (entity.Id > 0)
            {
                var ent = DbHelper.Get<StorageSupplier>(entity.Id);
                ent.Debt -= pay;
                if (ent.Debt < 0) {
                    ent.Debt = 0;
                }
                ent.Payment = ent.Total-ent.Debt;
                ent.IsInvoice = ent.IsInvoice;
                ent.Update(true);
            }
            else {
              
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑货款成功!", "StorageSupplier", "Index");
        }


        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<StorageSupplier>(id));
        }
    }
}
