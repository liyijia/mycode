using System.Linq;
using System.Web.Mvc;
using LY.EMIS5.Common;
using LY.EMIS5.Common.Mvc;
using NHibernate.Extensions.Data;
using System.Data;
using LY.EMIS5.Common.Mvc.Extensions;
using LY.EMIS5.Entities.Core.Stock;

namespace LY.EMIS5.Admin.Controllers
{
    public class SupplierController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string name = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Supplier> query = DbHelper.Query<Supplier>();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderByDescending(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c=> new {c.Id,c.Account,c.AccountNumber,c.Bank,c.Grade,c.IsInvoice,c.Name,c.Pnone,
                    Payment=DbHelper.Query<StorageSupplier>(m=>m.Supplier.Id==c.Id).Sum(m=>m.Payment),
                    Total= DbHelper.Query<StorageSupplier>(m => m.Supplier.Id == c.Id).Sum(m => m.Total),
                    Debt= DbHelper.Query<StorageSupplier>(m => m.Supplier.Id == c.Id).Sum(m => m.Debt)
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            if (id > 0) {
                return View(DbHelper.Get<Supplier>(id));
            }
            return View(new Supplier());
        }

        [HttpPost, Authorize]
        public ActionResult Create(Supplier entity)
        {
            if (entity.Id > 0)
            {
                entity.Update(true);
            }
            else {
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑供应商成功!", "Supplier", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Supplier>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除供应商成功!", "Supplier", "Index");
        }
        
    }
}
