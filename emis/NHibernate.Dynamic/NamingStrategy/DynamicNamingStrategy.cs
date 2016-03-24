using NHibernate.Cfg;
using NHibernate.Extensions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NHibernate.Extensions.NamingStrategy
{
    public class DynamicNamingStrategy : INamingStrategy
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        public static readonly DynamicNamingStrategy Instance = new DynamicNamingStrategy();

        private DynamicNamingStrategy()
        {

        }

        string INamingStrategy.ClassToTableName(string className)
        {
            var setting = DynamicSettingHelper.Settings[className];
            if (setting.NamingStrategy != null)
                return setting.NamingStrategy.ClassToTableName(className);
            return DefaultNamingStrategy.Instance.ClassToTableName(className);
        }

        string INamingStrategy.ColumnName(string columnName)
        {
            return DefaultNamingStrategy.Instance.ColumnName(columnName);
        }

        string INamingStrategy.LogicalColumnName(string columnName, string propertyName)
        {
            return DefaultNamingStrategy.Instance.LogicalColumnName(columnName, propertyName);
        }

        string INamingStrategy.PropertyToColumnName(string propertyName)
        {
            return DefaultNamingStrategy.Instance.PropertyToColumnName(propertyName);
        }

        string INamingStrategy.PropertyToTableName(string className, string propertyName)
        {
            return DefaultNamingStrategy.Instance.PropertyToTableName(className, propertyName);
        }

        string INamingStrategy.TableName(string tableName)
        {
            return DefaultNamingStrategy.Instance.TableName(tableName);
        }
    }
}
