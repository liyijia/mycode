using NHibernate;
using NHibernate.Cfg;
using NHibernate.Extensions.EventListener;
using NHibernate.Extensions.NamingStrategy;
using NHibernate.Intercept;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using NHibernate.Caches.Redis;
using System.Data;

namespace NHibernate.Extensions.Data
{
    internal class DbContext : IDisposable
    {
        private static readonly string _NhibernateCfgFile = "hibernate.cfg";
        private Configuration _Cfg = null;
        private ISessionFactory _SessionFactory = null;
        private static int _RedisInitializeFlag = 0;

        public string Id { get; private set; }

        public string DbName { get; private set; }

        internal static ILifeCycle GetInterceptor(IEntityObject entity)
        {
            if (entity == null)
                return null;
            var setting = DynamicSettingHelper.LoadFrom(entity.GetType());
            return setting == null ? null : setting.Interceptor;
        }

        internal static string GenerateId(IEnumerable<IDynamicNamingStrategy> namingStrategies = null, string dbname = null)
        {
            var id = dbname + ".";
            if (namingStrategies != null && namingStrategies.Count() > 0)
            {
                foreach (var strategy in namingStrategies)
                {
                    id += strategy.BasedOn + ";";
                }
            }
            return id;
        }

        public void BulidDatabase(bool dropIfExists = false)
        {
            var se = new SchemaExport(_Cfg);
            if (dropIfExists)
                se.Drop(true, true);
            se.Create(true, true);
        }

        internal DbContext(IEnumerable<IDynamicNamingStrategy> namingStrategies = null, string dbname = null)
        {
            //if (Interlocked.CompareExchange(ref _RedisInitializeFlag, 1, 0) == 0)
            //{
            //    var redisServerAddress = System.Configuration.ConfigurationManager.AppSettings["redisServerAddress"];
            //    if (!string.IsNullOrWhiteSpace(redisServerAddress))
            //        RedisCacheProvider.SetClientManager(new PooledRedisClientManager(redisServerAddress));
            //}

            this.DbName = dbname;
            this.Id = namingStrategies == null ? dbname : GenerateId(namingStrategies, dbname);

            var cfgFile = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase).Remove(0, 6) + "\\" + _NhibernateCfgFile + (string.IsNullOrWhiteSpace(dbname) ? "" : ("." + dbname)) + ".xml";
            _Cfg = new Configuration().SetNamingStrategy(DynamicNamingStrategy.Instance);
            _Cfg.SetListener(NHibernate.Event.ListenerType.PreDelete, new PreDeleteEventListener());
            _Cfg.SetListener(NHibernate.Event.ListenerType.PreInsert, new PreInsertEventListener());
            _Cfg.SetListener(NHibernate.Event.ListenerType.PostLoad, new PostLoadEventListener());
            _Cfg.SetListener(NHibernate.Event.ListenerType.PostInsert, new PostInsertEventListener());
            _Cfg.SetListener(NHibernate.Event.ListenerType.PostDelete, new PostDeleteEventListener());

            _Cfg.SetListener(NHibernate.Event.ListenerType.PreUpdate, new PreUpdateEventListener());
            _Cfg.SetListener(NHibernate.Event.ListenerType.PostUpdate, new PostUpdateEventListener());
            _Cfg.Configure(cfgFile);
            _SessionFactory = _Cfg.BuildSessionFactory();
        }

        public ISession OpenSession()
        {
            ISession session;
            if (SessionContext.Current.Sessions.TryGetValue(this.Id, out session))
            {
                if (session.Connection.State == ConnectionState.Closed || session.Connection.State == ConnectionState.Broken)
                {
                    session.Dispose();
                    session = null;
                    SessionContext.Current.Sessions.Remove(this.Id);
                }
            }
            if (session == null)
            {
                session = _SessionFactory.OpenSession();
                SessionContext.Current.Sessions.Add(this.Id, session);
            }
            return session;
        }

        public void CloseSession()
        {
            ISession session;
            if (SessionContext.Current.Sessions.TryGetValue(this.Id, out session))
            {
                session.Dispose();
            }
        }

        public bool IsDisposed { get; protected set; }

        public virtual void Dispose()
        {
            if (!IsDisposed)
            {
                CloseSession();
                if (_SessionFactory != null)
                    _SessionFactory.Dispose();
                IsDisposed = true;
            }
        }

        public T Get<T>(object id) where T : IEntityObject
        {
            try
            {
                return OpenSession().Load<T>(id);
            }
            catch (ObjectNotFoundException)
            {
                return default(T);
            }
        }

        public T Delete<T>(T entity, bool flush = false) where T : IEntityObject
        {
            try
            {
                var session = OpenSession();
                session.Delete(entity);
                if (flush)
                    session.Flush();
            }
            catch (ObjectNotFoundException)
            {

            }
            return entity;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null) where T : IEntityObject
        {
            var query = OpenSession().Query<T>();
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }

        /// <summary>
        /// 使用SqlBulkCopy批量插入
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <param name="data">DataTable数据源</param>
        /// <param name="copyOptions"><see cref="SqlBulkCopyOptions"/></param>
        public void BulkCopy(string tableName, System.Data.DataTable data, SqlTransaction transaction = null)
        {
            BulkCopy(tableName, data, null, transaction);
        }

        /// <summary>
        /// 使用SqlBulkCopy批量插入
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <param name="data">DataTable数据源</param>
        /// <param name="columnMapping">DataTable列和数据库表列映射关系集合</param>
        /// <param name="copyOptions"><see cref="SqlBulkCopyOptions"/></param>
        public void BulkCopy(string tableName, System.Data.DataTable dataTable, IEnumerable<SqlBulkCopyColumnMapping> columnMapping = null, SqlTransaction transaction = null)
        {
            using (SqlBulkCopy bulkCopy = transaction == null ?
                new SqlBulkCopy(this.OpenSession().Connection as SqlConnection) :
                new SqlBulkCopy(this.OpenSession().Connection as SqlConnection, SqlBulkCopyOptions.FireTriggers, transaction)
            )
            {
                bulkCopy.DestinationTableName = tableName;
                if (columnMapping == null || columnMapping.Count() == 0)
                {
                    foreach (System.Data.DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                }
                else
                {
                    foreach (var map in columnMapping)
                    {
                        bulkCopy.ColumnMappings.Add(map);
                    }
                }

                bulkCopy.WriteToServer(dataTable);
            }
        }

        public T Save<T>(T entity, bool flush = false) where T : IEntityObject
        {
            var session = OpenSession();
            session.Save(entity);
            if (flush)
                session.Flush();
            return entity;
        }

        public IEnumerable<T> SaveMany<T>(IEnumerable<T> list) where T : IEntityObject
        {
            var session = OpenSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var ent in list)
                {
                    session.Save(ent);
                }

                transaction.Commit();
            }

            return list;
        }

        public T Update<T>(T entity, bool flush = false) where T : IEntityObject
        {
            var session = OpenSession();
            session.Update(entity);
            if (flush)
                session.Flush();
            return entity;
        }

        public int ExecuteNonQuery<T>(string commandText) where T : IEntityObject
        {
            var session = OpenSession();
            var retVal = session.CreateSQLQuery(commandText).ExecuteUpdate();
            return retVal;
        }

        public int ExecuteScale<T>(string commandText) where T : IEntityObject
        {
            var session = OpenSession();
            var retVal = session.CreateSQLQuery(commandText).UniqueResult<int>();
            return retVal;
        }

        public IList<T> ExecuteQuery<T>(string commandText) where T : IEntityObject
        {
            return OpenSession().CreateSQLQuery(commandText).AddEntity(typeof(T)).List<T>();
        }
    }
}
