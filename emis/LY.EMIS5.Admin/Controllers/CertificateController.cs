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

namespace LY.EMIS5.Admin.Controllers
{
    public class CertificateController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            ViewBag.Companys = DbHelper.Query<Company>().AsSelectItemList(c => c.Name, c => c.Name);
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", string company = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Certificate> query = DbHelper.Query<Certificate>();
            if (!string.IsNullOrEmpty(company))
            {
                query = query.Where(c => c.Company.Contains(company));
            }
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.OrderBy(c => c.Id).Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    c.Post,
                    c.Major,
                    c.Name,
                    AnnualVerificationDate=c.AnnualVerificationDate.ToYearMonthDayString(),
                    c.Remarks,
                    Edit = c.Manager.Id==ManagerImp.Current.Id || ManagerImp.Current.Kind == "管理员"
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create(int id=0)
        {
            ViewBag.Companys = DbHelper.Query<Company>().ToList();
            if (id > 0) {
                return View(DbHelper.Get<Certificate>(id));
            }
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Create(Certificate entity)
        {
            if (entity.Id > 0)
            {
                var ent = DbHelper.Get<Certificate>(entity.Id);
                ent.AnnualVerificationDate = entity.AnnualVerificationDate;
                ent.Major = entity.Major;
                ent.Name = entity.Name;
                ent.Post = entity.Post;
                ent.Remarks = entity.Remarks;
                ent.Company = entity.Company;
                ent.Update(true);
            }
            else {
                entity.CreateDate = DateTime.Now;
                entity.Manager = ManagerImp.Current;
                entity.Save(true);
            }
            return this.RedirectToAction(100, "操作成功", "编辑证书成功!", "Certificate", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id = 0)
        {
            DbHelper.Get<Certificate>(id).Delete(true);
            return this.RedirectToAction(100, "操作成功", "删除证书成功!", "Certificate", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult View(int id)
        {
            return View(DbHelper.Get<Certificate>(id));
        }

        [HttpGet, Authorize]
        public ActionResult Report()
        {
            ViewBag.Companys = DbHelper.Query<Company>().ToList();
            return View(DbHelper.Query<Certificate>().ToList());
        }

    }
}
