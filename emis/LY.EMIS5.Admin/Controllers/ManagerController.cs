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
    public class ManagerController : Controller
    {


        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public string Index(string txt = "", int iDisplayStart = 0, int iDisplayLength = 15, string sSortDir_0 = "desc", string sEcho = "")
        {
            IQueryable<Manager> query = DbHelper.Query<Manager>();
            if (!string.IsNullOrWhiteSpace(txt))
                query = query.Where(m => m.Name.Contains(txt) || m.UserName.Contains(txt) || m.Phone == txt);
            return new PagedQueryResult<object>(iDisplayLength, iDisplayStart,
                query.Count(),
                query.Skip(iDisplayStart).Take(iDisplayLength).ToList().Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    UserName = c.UserName,
                    Phone = c.Phone,
                    Email = c.Company==null?"":c.Company,
                    Roles = c.Kind,
                    Stat = !c.IsEnabled ? "禁用" : "启用"
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create()
        {
            ViewBag.Companys = DbHelper.Query<Company>().ToList();
            return View(new Manager());
        }

        [HttpPost, Authorize]
        public ActionResult Create(Manager ent, string password)
        {
            ent.Password = new Cipher() { Value = password, SecurityMode = Common.Const.SecurityModes.MD5 }.Encrypt();
            ent.CreateTime = DateTime.Now;
            ent.IsEnabled = true;

            ent.Save(true);
            return this.RedirectToAction(100, "操作成功", "添加用户成功", "Manager", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.Companys = DbHelper.Query<Company>().ToList();
            return View(DbHelper.Get<Manager>(id));
        }

        [HttpPost, Authorize]
        public ActionResult Edit(Manager ent)
        {
            var entity = DbHelper.Get<Manager>(ent.Id);
            entity.Company = ent.Company;
            entity.Email = ent.Email;
            entity.Kind = ent.Kind;
            entity.Name = ent.Name;
            entity.Phone = ent.Phone;
            entity.Sex = ent.Sex;
            entity.UserName = ent.UserName;
            entity.Update(true);
            return this.RedirectToAction(100, "操作成功", "编辑用户成功", "Manager", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult EditPassword()
        {
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult EditPassword(string oldpassword,string newpassword)
        {
            if (ManagerImp.Current.Password.Value == new Cipher() { Value = oldpassword, SecurityMode = Common.Const.SecurityModes.MD5 }.Encrypt().Value) {
                ManagerImp.Current.Password = new Cipher() { Value = newpassword, SecurityMode = Common.Const.SecurityModes.MD5 }.Encrypt();
                ManagerImp.Current.Update(true);
                return this.RedirectToAction(100, "操作成功", "密码修改成功", "Manager", "EditPassword");
            }
            return this.RedirectToAction(100, "操作失败", "原密码不正确", "Manager", "EditPassword");
        }

        [HttpGet, Authorize]
        public ActionResult Personal()
        {
            return View(ManagerImp.Current);
        }
        /// <summary>
        /// 个人设置
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public JsonResult Personal(Manager manager)
        {
            using (var ts = TransactionScopes.Default)
            {
                if (manager == null)
                    return Util.Echo(Const.IconFlags.error, "保存失败", "无保存的对象");
                if (manager.Id != ManagerImp.Current.Id)
                    return Util.Echo(Const.IconFlags.error, "保存失败", "没有权限修改其他用户的资料");
                var oldEnt = DbHelper.Get<Manager>(manager.Id);
                if (oldEnt == null)
                    return Util.Echo(Const.IconFlags.error, "操作出错", string.Format("无法编辑用户{0}, 数据库中无对应数据!", manager.Name));
                int id = 0;

                oldEnt.Kind = manager.Kind;
                oldEnt.Name = manager.Name;
                oldEnt.Phone = manager.Phone;
                oldEnt.Email = manager.Email;
                oldEnt.Sex = manager.Sex;
                oldEnt.Update();
                ts.Complete();
                return Util.Echo(Const.IconFlags.success, "编辑用户", string.Format("编辑用户{0}, 操作成功!", manager.Name), "/Manager/Personal");
            }
        }
    
        public JsonResult Reset(int id)
        {
            var entity = DbHelper.Get<Manager>(id);
            entity.Password = new Cipher() { SecurityMode = Common.Const.SecurityModes.MD5, Value = "123456" }.Encrypt();
            entity.Update(true);
            return Util.Echo(100, "操作成功", "重置成功");
        }

        public JsonResult Enabled(int id)
        {
            var entity = DbHelper.Get<Manager>(id);
            entity.IsEnabled = !entity.IsEnabled;
            entity.Update(true);
            return Util.Echo(100, "操作成功", entity.IsEnabled ? "启用成功" : "禁用成功");
        }


        public JsonResult CheckUserName(string userName)
        {
            bool isExists = DbHelper.Query<Manager>(m => m.UserName == userName).Any();

            return Json(isExists, JsonRequestBehavior.AllowGet);
        }

    }
}
