using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NHibernate.Extensions
{
    /// <summary>
    /// 事务隔离级别
    /// </summary>
    public static class TransactionScopes
    {
        //下面是重复使用的对象，最好是缓存起来
        private static readonly TransactionOptions TransactionOptions_ReadCommitted = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
        private static readonly TransactionOptions TransactionOptions_ReadUncommitted = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted };
        private static readonly TransactionOptions TransactionOptions_RepeatableRead = new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead };
        private static readonly TransactionOptions TransactionOptions_Serializable = new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable };
        private static readonly TransactionOptions TransactionOptions_Snapshot = new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot };

        public static TransactionScope New(IsolationLevel? isolationLevel = null)
        {
            switch (isolationLevel)
            {
                case IsolationLevel.ReadUncommitted:
                    return ReadUncommitted;
                case IsolationLevel.RepeatableRead:
                    return RepeatableRead;
                case IsolationLevel.Serializable:
                    return Serializable;
                case IsolationLevel.Snapshot:
                    return Snapshot;
                default:
                    return ReadCommitted;
            }
        }

        /// <summary>
        /// 默认为ReadCommitted。ReadCommitted：不可以在事务期间读取可变数据，但是可以修改它。
        /// </summary>
        public static TransactionScope Default
        {
            get { return ReadCommitted; }
        }

        /// <summary>
        /// 不可以在事务期间读取可变数据，但是可以修改它。
        /// </summary>
        public static TransactionScope ReadCommitted
        {
            get { return new TransactionScope(TransactionScopeOption.Required, TransactionOptions_ReadCommitted); }
        }

        /// <summary>
        /// 可以在事务期间读取和修改可变数据。
        /// </summary>
        public static TransactionScope ReadUncommitted
        {
            get { return new TransactionScope(TransactionScopeOption.Required, TransactionOptions_ReadUncommitted); }
        }

        /// <summary>
        /// 可以在事务期间读取可变数据，但是不可以修改。可以在事务期间添加新数据。
        /// </summary>
        public static TransactionScope RepeatableRead
        {
            get { return new TransactionScope(TransactionScopeOption.Required, TransactionOptions_RepeatableRead); }
        }

        /// <summary>
        /// 可以在事务期间读取可变数据，但是不可以修改，也不可以添加任何新数据。
        /// </summary>
        public static TransactionScope Serializable
        {
            get { return new TransactionScope(TransactionScopeOption.Required, TransactionOptions_Serializable); }
        }

        /// <summary>
        /// 可以读取可变数据。在事务修改数据之前，它验证在它最初读取数据之后另一个事务是否更改过这些数据。如果数据已被更新，则会引发错误。这样使事务可获取先前提交的数据值。
        /// 在尝试提升以此隔离级别创建的事务时，将引发一个 InvalidOperationException，并产生错误信息“Transactions with IsolationLevel Snapshot cannot be promoted”（无法提升具有 IsolationLevel 快照的事务）。
        /// </summary>
        public static TransactionScope Snapshot
        {
            get { return new TransactionScope(TransactionScopeOption.Required, TransactionOptions_Snapshot); }
        }
    }
}
