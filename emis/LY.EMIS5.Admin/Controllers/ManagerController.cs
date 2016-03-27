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
                    Email = c.Email,
                    Roles = c.Kind,
                    Stat = !c.IsEnabled ? "禁用" : "启用"
                }).ToList<object>()) { }.ToDataTablesResult(sEcho);
        }

        

        [HttpGet, Authorize]
        public ActionResult Create()
        {
            var ent = new Manager();

            return View(ent);
        }

        [HttpPost, Authorize]
        public ActionResult Create(Manager ent, string password)
        {
            ent.Password = new Cipher() { Value = password, SecurityMode = Common.Const.SecurityModes.MD5 }.Encrypt();
            ent.CreateTime = DateTime.Now;
            ent.IsEnabled = true;

            ent.Save(true);
            return this.RedirectToAction(100, "操作成功", string.Format("添加新用户{0}, 操作成功!", ent.Name), "Manager", "Index");
        }

        [HttpGet, Authorize]
        public ActionResult Edit(int id = 0)
        {
            var ent = DbHelper.Query<Manager>(m => m.Id == id).FirstOrDefault();
            if (ent == null)
            {
                ent = new Manager();
            }
            return View(ent);
        }

        [HttpPost, Authorize]
        public ActionResult Edit(Manager ent, string MemberOf_Id, string Roles_Id)
        {
            if (ent == null)
            {
                return Util.Echo(Const.IconFlags.error, "保存失败", "无保存的对象");
            }

            var oldEnt = DbHelper.Query<Manager>(m => m.Id == ent.Id).FirstOrDefault();
            if (oldEnt == null)
            {
                return Util.Echo(Const.IconFlags.error, "操作出错", string.Format("无法编辑用户{0}, 数据库中无对应数据!", ent.Name));
            }

            oldEnt.Kind = ent.Kind;
            oldEnt.Name = ent.Name;
            oldEnt.UserName = ent.UserName;
            oldEnt.Phone = ent.Phone;
            oldEnt.Email = ent.Email;
            oldEnt.Sex = ent.Sex;
            oldEnt.Update(true);

            return Util.Echo(Const.IconFlags.success, "编辑用户", string.Format("编辑用户{0}, 操作成功!", ent.Name));
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

        //批量删除
        public JsonResult Delete(List<Manager> obj)
        {
            if (obj == null)
                return Util.Echo(0, "操作失败", "请选择删除对象");
            var Ids = obj.Select(s => s.Id).ToList();
            var List = DbHelper.Query<Manager>(s => Ids.Contains(s.Id)).ToList();
            using (var ts = TransactionScopes.Default)
            {
                foreach (var item in List)
                {
                    item.Delete();
                }
                ts.Complete();
            }
            return Util.Echo(100, "操作成功", "删除成功");


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

        [HttpGet, Authorize]
        public ActionResult Import()
        {
            var Table = new DataTable("管理员导入模版");
            Table.Columns.Add("登录名");
            Table.Columns.Add("姓名");
            Table.Columns.Add("密码");
            Table.Columns.Add("电话");
            Table.Columns.Add("邮箱");
            return Table.ExportToXls(Table.TableName + ".xls");
        }

        [HttpPost, Authorize]
        public ActionResult Import(int id = 0)
        {
            var Table = Request.Files["file"].ImportFromXls();
            if (Table.Rows.Count == 0)
                return Util.Echo(0, "导入数据格式错误", "导入数据格式错误");
            var error = "";
            try
            {
                using (var ts = TransactionScopes.Default)
                {
                    foreach (DataRow tr in Table.Rows)
                    {
                        if (!string.IsNullOrEmpty(tr["登录名"].ToString()))
                        {
                            if (DbHelper.Query<Manager>(c => c.UserName == tr["登录名"].ToString()).Count() == 0)
                            {
                                new Manager() { UserName = tr["登录名"].ToString(), Name = tr["姓名"].ToString(), Password = new Cipher() { SecurityMode = Common.Const.SecurityModes.MD5, Value = tr["密码"].ToString() }.Encrypt(), Phone = tr["电话"].ToString(), Email = tr["邮箱"].ToString() }.Save();
                            }
                            else
                            {
                                error += tr["登录名"].ToString() + ",";
                            }
                        }
                    }
                    ts.Complete();
                }
            }
            catch (Exception)
            {
                return Util.Echo(0, "导入数据格式错误", "导入数据格式错误");
            }
            if (error.Length > 0)
            {
                return Util.Echo(100, "导入成功", "导入成功，用户名" + error.TrimEnd(',') + "已存在");
            }
            else
            {
                return Util.Echo(100, "导入成功", "导入成功");
            }
        }



    }
}
