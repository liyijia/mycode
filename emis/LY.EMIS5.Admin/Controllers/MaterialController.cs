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
    public class MaterialController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string name = "", string type = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Material> query = DbHelper.Query<Material>();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(c => c.Type== type);
            }
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderBy(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    c.Name,
                    c.Type,
                    c.Spec,
                    c.Brand,
                    c.Stock,
                    InDate = c.InDate.ToYearMonthDayString()
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            if (id > 0) {
                return View(DbHelper.Get<Material>(id));
            }
            return View(new Material());
        }

        [HttpPost, Authorize]
        public ActionResult Create(Material entity)
        {
            if (entity.Id > 0)
            {
                var ent = DbHelper.Get<Material>(entity.Id);
                ent.Brand = entity.Brand;
                ent.Manufactor = entity.Manufactor;
                ent.Name = entity.Name;
                ent.Remark = entity.Remark;
                ent.Spec = entity.Spec;
                ent.Stock = entity.Stock;
                ent.Type = entity.Type;
                ent.Update(true);
            }
            else {
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑材料成功!", "Material", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Material>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除材料成功!", "Material", "Index");
        }
        
    }
}
