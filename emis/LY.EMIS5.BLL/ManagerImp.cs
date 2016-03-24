using LY.EMIS5.Common;
using LY.EMIS5.Common.Const;
using NHibernate.Extensions.Data;
using LY.EMIS5.Entities.Core.Memberships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Extensions;
using System.Web;
using LY.EMIS5.Common.Security;
using LY.EMIS5.Const;
using LY.EMIS5.Common.Exceptions;

namespace LY.EMIS5.BLL
{
    public class ManagerImp : LifeCycle<Manager>
    {
        public override void OnLoad(Manager entity)
        {
            entity.Password.IsEncrypted = true;
            entity.Password.SecurityMode = SecurityModes.MD5;
        }

        public override bool OnSave(Manager entity)
        {
            entity.Password.IsEncrypted = false;
            entity.Password.SecurityMode = SecurityModes.MD5;
            entity.Password = entity.Password.Encrypt();

            return false;
        }

        public static Manager Current
        {
            get
            {
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    return HttpContext.Current.User as Manager;
                return new Manager();
            }
        }

        public static Manager Get(int id)
        {
            return DbHelper.Get<Manager>(id);
        }



        public static Manager Login(string UserName, string PassWord)
        {
            string error = "";
            var passwordHash = new Cipher { Value = PassWord, SecurityMode = SecurityModes.MD5 };

            var manager = DbHelper.Query<Manager>(m => m.UserName == UserName).FirstOrDefault();
            if (manager == null)
                error = "用户名不存在";
            else if (passwordHash.Encrypt().Value != manager.Password.Value)
                error = "密码不正确";
            else if (!manager.IsEnabled)
                error = "该账号已被禁用";
            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new EmisException(102, "认证失败", error);
            }
            return manager;
        }

    }
}
