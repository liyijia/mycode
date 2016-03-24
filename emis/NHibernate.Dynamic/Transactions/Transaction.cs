using NHibernate.Extensions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Extensions.Transactions
{
    public class Transaction : IDisposable
    {
        private ISession session;
        private ITransaction transaction;

        private bool isCompleted = false;

        public Transaction(ISession session)
        {
            if (System.Transactions.Transaction.Current == null)
                transaction = session.BeginTransaction();
        }

        public void Complete()
        {
            if (transaction != null && transaction.WasRolledBack)
                throw new TransactionException("transaction was rolled back");
            else
                isCompleted = true;
        }

        public void Dispose()
        {
            if (isCompleted && transaction != null)
            {
                this.session.Flush();
                transaction.Commit();
            }
        }
    }
}
