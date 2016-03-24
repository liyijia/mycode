using NHibernate.Extensions.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Specialized;

namespace NHibernate.Extensions
{
    internal class SessionContext : IDisposable
    {
        private static readonly ConcurrentDictionary<string, DbContext> _DbContexts = new ConcurrentDictionary<string, DbContext>();
        private static readonly string HttpSessionContextKey = "_Key.NHibernate.Session.Context";
        private static IDictionary Cache { get { return HttpContext.Current.Items; } }

        [ThreadStatic]
        private static SessionContext InstanceInThread;

        public static SessionContext Current
        {
            get
            {
                if (HttpContext.Current == null) //win
                {
                    if (InstanceInThread == null || InstanceInThread.IsDisposed)
                        InstanceInThread = new SessionContext();
                    return InstanceInThread;
                }
                else //web
                {
                    if (Cache.Contains(HttpSessionContextKey))
                        return Cache[HttpSessionContextKey] as SessionContext;
                    else
                    {
                        var context = new SessionContext();
                        Cache.Add(HttpSessionContextKey, context);
                        return context;
                    }
                }
            }
        }

        internal ConcurrentDictionary<string, DbContext> DbContexts { get { return _DbContexts; } }

        public IDictionary<string, ISession> Sessions { get; private set; }

        public IDictionary<string, IDbConnection> Connections { get; private set; }

        public NameValueCollection NamingStrategies { get; private set; }

        private SessionContext()
        {
            this.Sessions = new Dictionary<string, ISession>();
            this.Connections = new Dictionary<string, IDbConnection>();
            this.NamingStrategies = new NameValueCollection();

            if (HttpContext.Current != null && HttpContext.Current.ApplicationInstance != null)
            {
                HttpContext.Current.ApplicationInstance.RequestCompleted += (sender, e) => { this.Dispose(); };
            }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (var session in this.Sessions.Values)
                {
                    session.Dispose();
                }
                foreach (var connection in this.Connections.Values)
                {
                    connection.Dispose();
                }
                this.Sessions.Clear();
                this.Connections.Clear();
                this.NamingStrategies.Clear();

                if (InstanceInThread != null && !InstanceInThread.IsDisposed)
                {
                    InstanceInThread.Dispose();
                    InstanceInThread = null;
                }

                this.IsDisposed = true;
            }
        }
    }
}
