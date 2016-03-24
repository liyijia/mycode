using NHibernate;
using NHibernate.Connection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NHibernate.Extensions.Connection
{
    public class CachedDriverConnectionProvider : ConnectionProvider
    {
        public override void CloseConnection(IDbConnection conn)
        {
        }

        public override IDbConnection GetConnection()
        {
            IDbConnection conn;
            if (SessionContext.Current.Connections.TryGetValue(ConnectionString, out conn))
            {
                if (conn.State != ConnectionState.Closed && conn.State != ConnectionState.Broken)
                    return conn;
                conn.Dispose();
                SessionContext.Current.Connections.Remove(ConnectionString);
            }
            conn = Driver.CreateConnection();
            try
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
            }
            catch (Exception)
            {
                conn.Dispose();
                throw;
            }
            SessionContext.Current.Connections.Add(ConnectionString, conn);
            return conn;
        }
    }
}
