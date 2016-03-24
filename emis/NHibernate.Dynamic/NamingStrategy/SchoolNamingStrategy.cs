using NHibernate.Cfg;
using NHibernate.Extensions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NHibernate.Extensions.NamingStrategy
{
    public class SchoolNamingStrategy : IDynamicNamingStrategy
    {
        public static readonly IDynamicNamingStrategy Instance = new SchoolNamingStrategy();

        public string Key { get { return "__" + this.GetType().ToString() + ".BasedOn"; } }

        public string BasedOn
        {
            get { return SessionContext.Current.NamingStrategies[Key]; }
            set { SessionContext.Current.NamingStrategies[Key] = value; }
        }

        private SchoolNamingStrategy()
        {
            this.BasedOn = "";
        }

        public string ClassToTableName(string className)
        {
            var setting = DynamicSettingHelper.Settings[className];
            if (string.IsNullOrWhiteSpace(this.BasedOn))
                return setting.TableName.TrimEnd('_');
            return setting.TableName.TrimEnd('_') + "_" + this.BasedOn;
        }
    }
}
