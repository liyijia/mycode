using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Expressions;
using System.Transactions;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Extensions;
using NHibernate.Extensions.Connection;
using System.Data.SqlClient;
using NHibernate.Transaction;

namespace NHibernate.Extensions.Data
{
    public static class DbHelper
    {
        private static IEnumerable<IDynamicNamingStrategy> _NamingStrategies;
        private static readonly FieldInfo _TransField = typeof(AdoTransaction).GetField("trans", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);

        public static void Configure(IEnumerable<IDynamicNamingStrategy> namingStrategies, params Assembly[] assemblies)
        {
            _NamingStrategies = namingStrategies;
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.IsClass)
                    {
                        var entity = type.GetInterface(typeof(IEntityObject).ToString());
                        if (entity != null)
                            DynamicSettingHelper.LoadFrom(type);
                        else
                        {
                            var interceptor = type.GetInterface(typeof(ILifeCycle<>).ToString().Replace("[T]", ""));
                            if (interceptor != null)
                            {
                                var genericType = interceptor.GetGenericArguments().First();
                                var setting = DynamicSettingHelper.LoadFrom(genericType);
                                setting.Interceptor = Activator.CreateInstance(type) as ILifeCycle;
                            }
                        }
                    }
                }
            }
            //分析继承树：没有定义拦截器的实体使用父类的拦截器
            foreach (var setting in DynamicSettingHelper.Settings.Values)
            {
                setting.Interceptor = InheritTest(setting);
            }
        }

        private static ILifeCycle InheritTest(DynamicSetting setting)
        {
            if (setting.Interceptor != null)
                return setting.Interceptor;

            var baseType = setting.EntityType.BaseType;
            var baseSetting = DynamicSettingHelper.Settings[baseType.ToString()];
            if (baseSetting != null)
            {
                setting.Interceptor = baseSetting.Interceptor;
                if (setting.Interceptor == null)
                    setting.Interceptor = InheritTest(baseSetting);
            }
            return setting.Interceptor;
        }

        private static DbContext GetDbContext(System.Type entityType)
        {
            var dynamicSetting = DynamicSettingHelper.Settings[entityType.ToString()];
            var key = DbContext.GenerateId(_NamingStrategies, dynamicSetting.DbName);

            DbContext dbContext;
            if (!SessionContext.Current.DbContexts.TryGetValue(key, out dbContext))
            {
                dbContext = new DbContext(_NamingStrategies, dynamicSetting.DbName);
                return SessionContext.Current.DbContexts.AddOrUpdate(key, dbContext, (k, v) => v);
            }
            return dbContext;
        }

        private static ISession OpenSession<T>() where T : IEntityObject
        {
            return GetDbContext(typeof(T)).OpenSession();
        }

        public static T Get<T>(object id) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).Get<T>(id);
        }

        public static T Delete<T>(this T entity, bool flush = false) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).Delete<T>(entity, flush);
        }
        public static IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).Query<T>(predicate);
        }
        public static T Save<T>(this T entity, bool flush = false) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).Save<T>(entity, flush);
        }

        /// <summary>
        /// 使用SqlBulkCopy批量插入
        /// </summary>
        /// <typeparam name="T">任意类型T DbContext构造方法需要</typeparam>
        /// <param name="tableName">数据库表名</param>
        /// <param name="data">DataTable数据源</param>
        /// <param name="copyOptions"><see cref="SqlBulkCopyOptions"/></param>
        public static void BulkCopy<T>(string tableName, System.Data.DataTable data, SqlTransaction transaction = null) where T : IEntityObject
        {
            GetDbContext(typeof(T)).BulkCopy(tableName, data, transaction);
        }

        /// <summary>
        /// 使用SqlBulkCopy批量插入
        /// </summary>
        /// <typeparam name="T">任意类型T DbContext构造方法需要</typeparam>
        /// <param name="tableName">数据库表名</param>
        /// <param name="data">DataTable数据源</param>
        /// <param name="columnMapping">DataTable列和数据库表列映射关系集合</param>
        /// <param name="copyOptions"><see cref="SqlBulkCopyOptions"/></param>
        public static void BulkCopy<T>(string tableName, System.Data.DataTable data, IEnumerable<SqlBulkCopyColumnMapping> columnMapping, SqlTransaction transaction = null) where T : IEntityObject
        {
            GetDbContext(typeof(T)).BulkCopy(tableName, data, columnMapping, transaction);
        }

        public static IEnumerable<T> SaveMany<T>(this IEnumerable<T> list) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).SaveMany(list);
        }

        public static T Update<T>(this T entity, bool flush = false) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).Update<T>(entity, flush);
        }

        public static int ExecuteNonQuery<T>(string commandText) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).ExecuteNonQuery<T>(commandText);
        }

        public static int ExecuteScale<T>(string commandText) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).ExecuteScale<T>(commandText);
        }

        public static void BuildDatabase(string cfgName = null, bool dropIfExists = false)
        {
            using (var ctx = new DbContext(null, cfgName))
            {
                ctx.BulidDatabase(dropIfExists);
            }
        }

        public static int ExecuteNonQuery<T>(string commandText, params System.Tuple<string, object>[] parameters) where T : IEntityObject
        {
            var session = OpenSession<T>();
            using (var cmd = session.Connection.CreateCommand())
            {
                cmd.CommandText = commandText;
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        var parameter = session.Connection.CreateCommand().CreateParameter();
                        parameter.ParameterName = p.Item1;
                        parameter.Value = p.Item2;
                        cmd.Parameters.Add(parameter);
                    }
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public static IList<T> ExecuteQuery<T>(string commandText) where T : IEntityObject
        {
            return GetDbContext(typeof(T)).ExecuteQuery<T>(commandText);
        }

        public static System.Data.IDataReader ExecuteReader<T>(string commandText, params System.Tuple<string, object>[] parameters) where T : IEntityObject
        {
            var session = OpenSession<T>();
            using (var cmd = session.Connection.CreateCommand())
            {
                cmd.CommandText = commandText;
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        var parameter = session.Connection.CreateCommand().CreateParameter();
                        parameter.ParameterName = p.Item1;
                        parameter.Value = p.Item2;
                        cmd.Parameters.Add(parameter);
                    }
                }
                return cmd.ExecuteReader();
            }
        }

        public static ITransaction BeginTransaction<T>(System.Data.IsolationLevel il = System.Data.IsolationLevel.ReadCommitted) where T : IEntityObject
        {
            var session = OpenSession<T>();
            return session.BeginTransaction(il);
        }

        public static SqlTransaction SqlTransaction(this ITransaction trans)
        {
            return _TransField.GetValue(trans) as SqlTransaction;
        }
    }
}
